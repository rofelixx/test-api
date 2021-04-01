using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Segfy.Schedule.Tests.Integration.DynamoDB.Model;

namespace Segfy.Schedule.Tests.Integration.DynamoDB
{
    public class AmazonDynamoDbFixture : IDisposable
    {
        public AmazonDynamoDBClient DbClient { get; private set; }

        public DummyTable EntityForSingle { get; set; }
        public DummyTable EntityForUpdate { get; set; }

        public AmazonDynamoDbFixture()
        {
            var clientConfig = new AmazonDynamoDBConfig
            {
                ServiceURL = "http://10.10.0.211:8000",
                Timeout = TimeSpan.FromSeconds(10),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 3
            };
            var credentials = new BasicAWSCredentials("xxx", "xxx");
            DbClient = new AmazonDynamoDBClient(credentials, clientConfig);

            var createTableRequest = new CreateTableRequest();
            createTableRequest.TableName = "dummy-table";
            createTableRequest.ProvisionedThroughput = new ProvisionedThroughput(3, 3);
            createTableRequest.KeySchema.Add(new KeySchemaElement("subscription_id", KeyType.HASH));
            createTableRequest.KeySchema.Add(new KeySchemaElement("id", KeyType.RANGE));

            createTableRequest.AttributeDefinitions.Add(new AttributeDefinition("subscription_id", ScalarAttributeType.S));
            createTableRequest.AttributeDefinitions.Add(new AttributeDefinition("id", ScalarAttributeType.S));
            createTableRequest.AttributeDefinitions.Add(new AttributeDefinition("dummy_index", ScalarAttributeType.S));

            var globalindex = new GlobalSecondaryIndex() { IndexName = "dummy_index" };
            globalindex.KeySchema.Add(new KeySchemaElement("dummy_index", KeyType.HASH));
            globalindex.ProvisionedThroughput = new ProvisionedThroughput(3, 3);
            globalindex.Projection = new Projection() { ProjectionType = ProjectionType.ALL };

            createTableRequest.GlobalSecondaryIndexes.Add(globalindex);
            var createtask = DbClient.CreateTableAsync(createTableRequest);
            createtask.GetAwaiter().GetResult();

            var repo = new DummyTableRepository(DbClient);
            var enities = new List<DummyTable>();
            for (int i = 0; i < 50; i++)
            {
                var a = new DummyTable()
                {
                    DummyIndex = "5753a917-18cb-4ccc-ac07-325e5a5da259",
                    DummyInteger = 1,
                    SubscriptionId = new Guid("39de3de4-c168-4caa-80d0-c1580d6c57f7"),
                    Text = "teste"
                };
                enities.Add(a);
            }

            var index = Guid.NewGuid().ToString();
            for (int i = 0; i < 10; i++)
            {
                if (i % 3 == 0)
                {
                    index = Guid.NewGuid().ToString();
                }
                var a = new DummyTable()
                {
                    DummyIndex = index,
                    DummyInteger = 2,
                    SubscriptionId = new Guid("cf6d71a7-97f3-4ed1-955c-aa538b2c4177"),
                    Text = "teste2"
                };
                enities.Add(a);

            }

            for (int i = 0; i < 10; i++)
            {
                var a = new DummyTable()
                {
                    DummyIndex = Guid.NewGuid().ToString(),
                    DummyInteger = 3,
                    SubscriptionId = Guid.NewGuid(),
                    Text = "teste 3"
                };
                enities.Add(a);
            }

            var taskAdded = repo.Add(enities);
            var arr = taskAdded.GetAwaiter().GetResult().ToArray();

            EntityForSingle = arr[0];
            EntityForUpdate = arr[1];
        }

        public void Dispose()
        {
            // clean up test data from the database
            var task = DbClient.DeleteTableAsync("dummy-table");
            task.Wait();
        }
    }
}
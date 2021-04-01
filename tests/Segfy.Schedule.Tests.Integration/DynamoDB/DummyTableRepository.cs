using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Segfy.Schedule.Tests.Integration.DynamoDB.Model;
using Segfy.Schedule.Infra.Repositories.Base;

namespace Segfy.Schedule.Tests.Integration.DynamoDB
{
    public class DummyTableRepository : TableRepository<DummyTable>
    {
        public DummyTableRepository(IAmazonDynamoDB client) : base(client)
        {
        }

    }
}
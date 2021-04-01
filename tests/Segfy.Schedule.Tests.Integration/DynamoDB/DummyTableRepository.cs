using Segfy.Schedule.Tests.Integration.DynamoDB.Model;
using Segfy.Schedule.Infra.Repositories.Base;
using Amazon.DynamoDBv2.DataModel;

namespace Segfy.Schedule.Tests.Integration.DynamoDB
{
    public class DummyTableRepository : TableRepository<DummyTable>
    {
        public DummyTableRepository(IDynamoDBContext context) : base(context)
        {
        }

    }
}
using Segfy.Schedule.Tests.Integration.DynamoDB.Model;
using Segfy.Schedule.Infra.Repositories.Base;
using Amazon.DynamoDBv2.DataModel;
using Segfy.Schedule.Infra.Operations;

namespace Segfy.Schedule.Tests.Integration.DynamoDB
{
    public class DummyTableRepository : TableRepository<DummyTable>
    {
        public DummyTableRepository(IDynamoBDOperations<DummyTable> context) : base(context)
        {
        }

    }
}
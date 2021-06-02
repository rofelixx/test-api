using Amazon.DynamoDBv2.DocumentModel;
using FluentAssertions;
using Moq;
using Segfy.Schedule.Infra.Handles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Segfy.Schedule.Tests.Infra.Handles
{
    public class DynamoBDFiltersHandleTests
    {
        [Fact()]
        public async Task ScheduleRepository_Add()
        {
            //Given
            IDynamoBDFiltersHandle dynamoBDFiltersHandle = new DynamoBDFiltersHandle();
            QueryFilter queryFilter = new QueryFilter();
            IList<Model.Filters.Filter> filters = new List<Model.Filters.Filter>
            {
                new Model.Filters.Filter
                {
                    Field= "recurrence",
                    Operator = Model.Enuns.OperatorType.Contains,
                    Value = new string[]{ "once" }
                },
                new Model.Filters.Filter
                {
                    Field= "created_at",
                    Operator = Model.Enuns.OperatorType.Between,
                    Value = new string[]{ "2021-04-07T17:38:08.227-03:00", "2021-04-09T17:38:08.227-03:00" }
                }
            };


            //When
            dynamoBDFiltersHandle.Apply(queryFilter, filters);

            //Then
            queryFilter.ToConditions().Should().HaveCount(2, "we passed 2 filters to dynamo db");
        }
    }
}

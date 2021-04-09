using Amazon.DynamoDBv2.DocumentModel;
using Segfy.Schedule.Model.Filters;
using System.Collections.Generic;

namespace Segfy.Schedule.Infra.Handles
{
    public interface IDynamoBDFilterHandles
    {
        void Apply(QueryFilter queryFilter, IList<Model.Filters.Filter> filters);
    }
}

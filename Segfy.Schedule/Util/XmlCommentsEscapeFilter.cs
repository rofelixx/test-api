using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Segfy.Schedule.Util
{
    internal class XmlCommentsEscapeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Description = operation.Description?.Replace("&amp;", "&");
            operation.Summary = operation.Summary?.Replace("&amp;", "&");
        }
    }
}

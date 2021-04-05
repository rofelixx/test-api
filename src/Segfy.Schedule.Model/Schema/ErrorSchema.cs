using System.Collections.Generic;

namespace Segfy.Schedule.Model.Schema
{
    public class ErrorSchema
    {
        public int StatusCode { get; set; }
        public bool IsModelValidatonError { get; set; }
        public IEnumerable<ValidationError> Errors { get; set; }
        public string ReferenceErrorCode { get; set; }
        public string ReferenceDocumentLink { get; set; }
        public object CustomError { get; set; }
        public bool IsCustomErrorObject { get; set; }
    }

    public class ValidationError
    {
        public string Name { get; }
        public string Reason { get; }
    }
}
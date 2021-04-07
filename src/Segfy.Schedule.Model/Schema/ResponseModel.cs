using System;
using System.Collections.Generic;

namespace Segfy.Schedule.Model.Schema
{
    public class ResponseModel
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public static ResponseModel Success(string msg = null)
        {
            return new ResponseModel()
            {
                IsSuccessful = true,
                Message = msg ?? "Successful",
            };
        }
    }

    public class ResponseModelSingle<T> : ResponseModel
    {
        public T Data { get; set; }

        public static ResponseModelSingle<T> Success(T data, string msg = null)
        {
            return new ResponseModelSingle<T>()
            {
                IsSuccessful = true,
                Message = msg ?? "Successful",
                Data = data,
            };
        }
    }

    public class ResponseModelMultiple<T> : ResponseModel
    {
        public IEnumerable<T> Data { get; set; }
        public Pagination PaginationDetails { get; set; }

        public static ResponseModelMultiple<T> Success(IEnumerable<T> data, Pagination pagination, string msg = null)
        {
            return new ResponseModelMultiple<T>()
            {
                IsSuccessful = true,
                Message = msg ?? "Successful",
                Data = data,
                PaginationDetails = pagination,
            };
        }
    }

    public class Pagination
    {
        public Guid? NextKey { get; set; }

    }
}
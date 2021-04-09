using Amazon.DynamoDBv2.DocumentModel;
using Segfy.Schedule.Model.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Segfy.Schedule.Infra.Handles
{
    public class DynamoBDFiltersHandle : IDynamoBDFiltersHandle
    {
        private readonly Dictionary<OperatorType, ScanOperator> operators = new Dictionary<OperatorType, ScanOperator>
        {
            {OperatorType.Equal,ScanOperator.Equal},
            {OperatorType.NotEqual,ScanOperator.NotEqual},
            {OperatorType.LessThanOrEqual,ScanOperator.LessThanOrEqual},
            {OperatorType.LessThan,ScanOperator.LessThan},
            {OperatorType.GreaterThanOrEqual,ScanOperator.GreaterThanOrEqual},
            {OperatorType.GreaterThan,ScanOperator.GreaterThan},
            {OperatorType.IsNotNull,ScanOperator.IsNotNull},
            {OperatorType.IsNull,ScanOperator.IsNull},
            {OperatorType.Contains,ScanOperator.Contains},
            {OperatorType.NotContains,ScanOperator.NotContains},
            {OperatorType.BeginsWith,ScanOperator.BeginsWith},
            {OperatorType.In,ScanOperator.In},
            {OperatorType.Between, ScanOperator.Between},
        };
        public void Apply(QueryFilter queryFilter, IList<Model.Filters.Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                foreach (Model.Filters.Filter userFilter in filters)
                {
                    queryFilter.AddCondition(userFilter.Field, operators[userFilter.Operator], ToEntries(userFilter.Value));
                }
            }
        }

        private DynamoDBEntry[] ToEntries(string[] values)
        {
            return values.Select(v => ToEntry(v)).ToArray();
        }

        private DynamoDBEntry ToEntry(string value)
        {
            if (value == null)
                return new DynamoDBNull();
            if (IsDateTime(value))
                return Convert.ToDateTime(value);
            return value.ToString();
        }

        private bool IsDateTime(string text)
        {
            DateTime dateTime;
            bool isDateTime = false;

            // Check for empty string.
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            isDateTime = DateTime.TryParse(text, out dateTime);

            return isDateTime;
        }
    }
}

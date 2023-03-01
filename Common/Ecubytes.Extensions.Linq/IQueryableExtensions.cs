using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ecubytes.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace System.Linq
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TSource> ApplyQueryRequest<TSource>(this IQueryable<TSource> source,
            QueryRequest request)
        {
            return source.ApplyQueryRequest(request, onlyCount: false);
        }

        public static IQueryable<TSource> ApplyQueryRequestCount<TSource>(this IQueryable<TSource> source,
            QueryRequest request)
        {
            return source.ApplyQueryRequest(request, onlyCount: true);
        }

        private static IQueryable<TSource> ApplyQueryRequest<TSource>(this IQueryable<TSource> source,
            QueryRequest request, bool onlyCount = false)
        {
            int? page = request.Page;
            int? pageSize = request.PageSize;

            var query = source;

            if (!request.MainConditionGroup.IsEmpty)
            {
                StringBuilder whereString = new StringBuilder("p => ");
                List<object> values = new List<object>();
                int index = 0;
                var config = new ParsingConfig { ResolveTypesBySimpleName = true };

                whereString.Append(GetExpressionFilterGroup<TSource>(request.MainConditionGroup, ref index, values));

                if (whereString.Length > 0)
                    query = query.Where(config, whereString.ToString(), values.ToArray());
            }

            if (!onlyCount)
            {
                if (request.SortFields.Any())
                {
                    StringBuilder orderByString = new StringBuilder();
                    foreach (var field in request.SortFields.OrderBy(p => p.SortIndex))
                    {
                        string propertyName = typeof(TSource).GetPropertyCaseInsensitive(field.FieldName).Name;

                        if (orderByString.Length > 0)
                            orderByString.Append(", ");

                        orderByString.Append(propertyName);

                        if (field.SortOrientation == SortOrientation.Descendent)
                            orderByString.Append(" desc");

                        // if (field.SortOrientation == SortOrientation.Ascendent)
                        //     query = query.OrderBy(p => EF.Property<object>(p, propertyName));
                        // else
                        //     query = query.OrderByDescending(p => EF.Property<object>(p, propertyName));
                    }

                    if (orderByString.Length > 0)
                        query = query.OrderBy(orderByString.ToString());
                }
            }


            // else
            // {
            //     query = query.OrderBy(p => p.Tradename);
            // }
            if (!onlyCount && pageSize.HasValue && page.HasValue)
                return query.Skip(pageSize.Value * (page.Value - 1)).Take(pageSize.Value);
            else
                return query;
        }

        private static string GetExpressionFilterGroup<TSource>(QueryFieldConditionGroup conditionGroup, ref int index, List<object> values)
        {
            StringBuilder expression = new StringBuilder();

            expression.Append(" ( "); // Start Group
            for (int i = 0; i < conditionGroup.Conditions.Count; i++)
            {
                if (i > 0)
                    expression.Append($" {GetLogicalOperatorString(conditionGroup.Operator)} ");

                var condition = conditionGroup.Conditions[i];
                expression.Append(GetExpressionFilter<TSource>(condition, ref index, values));
                index++;
            }

            foreach (var group in conditionGroup.ConditionGroups)
            {
                expression.Append(GetExpressionFilterGroup<TSource>(group, ref index, values));
            }

            expression.Append(" ) "); // End Group

            return expression.ToString();
        }

        private static string GetLogicalOperatorString(LogicalOperators logicalOperator)
        {
            if (logicalOperator == LogicalOperators.Or)
                return "or";
            else //(logicalOperator == LogicalOperators.And)
                return "and";
        }

        private static string GetExpressionFilter<TSource>(QueryFieldCondition condition, ref int index, List<object> values)
        {
            string expression = null;

            PropertyInfo pInfo = typeof(TSource).GetPropertyCaseInsensitive(condition.FieldName);
            if (pInfo == null)
                throw new ArgumentException($"Invalid FieldName '{condition.FieldName}' for {typeof(TSource).Name} using QueryRequest Filter");

            string propertyName = pInfo.Name;

            if (condition.IsCollection)
            {
                string expressionFormat = $"@{index}.Any({{0}})";

                if (condition.Operator == RelationalOperators.Contain)
                {
                    expression = $"@{index}.Any(p.{propertyName}.ToLower().Contains(it))";
                }
                else // Equal
                {
                    if (condition.DataType == typeof(Guid).Name)
                        expression = $"@{index}.Any(p.{propertyName}.Equals(it))";                        
                    else if (condition.DataType == typeof(String).Name)
                        expression = $"@{index}.Any(p.{propertyName}.ToLower() = it)";                        
                    else
                        expression = $"@{index}.Any(p.{propertyName} = it)";                        
                }
            }
            else
            {

                if (condition.Operator == RelationalOperators.Contain)
                {
                    expression = $"p.{propertyName}.ToLower().Contains(@{index})";
                }
                else // Equal
                {
                    if (condition.DataType == typeof(Guid).Name)
                        expression = $"p.{propertyName}.Equals(@{index})";
                    // else if (condition.DataType == typeof(String).Name)
                    //     expression = $"p.{propertyName}.ToLower() = @{index}";
                    else
                        expression = $"p.{propertyName} = @{index}";
                }
            }

            values.Add(GetValueFilter(condition));

            return expression;
        }

        private static object GetValueFilter(QueryFieldCondition field)
        {
            object valueFilter = field.Value;

            if (field.Value != null)
            {
                //string propertyName = typeof(TSource).GetPropertyCaseInsensitive(field.Name).Name;
                //valueFilter = Convert.ChangeType(field.Value, Type.GetType($"System.{field.DataType}"));
                if (valueFilter != null && field.DataType == typeof(String).Name && field.Operator == RelationalOperators.Contain)
                    valueFilter = valueFilter.ToString().ToLower();
            }

            return valueFilter;
        }

        // private static bool Parse<ElementType>(object value, out ElementType result)
        // {
        //     result = default(ElementType);

        //     //if (value == null) return false;
        //     if (string.IsNullOrWhiteSpace(value.FirstValue)) return false;

        //     try
        //     {
        //         result = (ElementType)Convert.ChangeType(value, typeof(ElementType));
        //         return true;
        //     }
        //     catch { return false; }
        // }
    }
}

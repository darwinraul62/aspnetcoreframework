using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecubytes.AspNetCore.Mvc.QueryRequestBinding;
using Ecubytes.Data.Common;
using Ecubytes.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Ecubytes.Data.Common
{
    public static class QueryRequestWebUtilities
    {
        public static string ToQueryString(this QueryRequest request)
        {
            return ToQueryString(request, string.Empty);
        }

        public static string ToQueryString(this QueryRequest request, string uri)
        {
            //Multiple Parameters
            var queryParams = new Dictionary<string, string>();

            if (request.Page.HasValue)
                queryParams.Add(QueryRequestPropertiesNames.Page, request.Page.ToString());

            if (request.PageSize.HasValue)
                queryParams.Add(QueryRequestPropertiesNames.PageSize, request.PageSize.ToString());

            queryParams.Add(QueryRequestPropertiesNames.SearchValue, request.SearchValue);

            int index = 0;
            if (request.SortFields.Any())
            {
                foreach (var field in request.SortFields)
                {
                    queryParams.Add($"{QueryFieldSortPropertiesNames.Entity}[{index}].{QueryFieldSortPropertiesNames.FieldName}",
                        field.FieldName);
                    queryParams.Add($"{QueryFieldSortPropertiesNames.Entity}[{index}].{QueryFieldSortPropertiesNames.SortIndex}",
                        field.SortIndex.ToString());
                    queryParams.Add($"{QueryFieldSortPropertiesNames.Entity}[{index}].{QueryFieldSortPropertiesNames.SortOrientation}",
                        ((int)field.SortOrientation).ToString());

                    index++;
                }
            }

            if (request.MainConditionGroup.Conditions.Any() || request.MainConditionGroup.ConditionGroups.Any())
            {
                var dicInnerGroups = GetConditionGroupParams($"{QueryFieldConditionGroupPropertiesNames.Entity}",
                    request.MainConditionGroup);

                foreach (var item in dicInnerGroups)
                    queryParams.Add(item.Key, item.Value);
            }


            return QueryHelpers.AddQueryString(uri, queryParams);
        }

        private static Dictionary<string, string> GetConditionGroupParams(string prefix, QueryFieldConditionGroup conditionGroup)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            int index = 0;

            queryParams.Add($"{prefix}.{QueryFieldConditionGroupPropertiesNames.Operator}",
                Convert.ToString((int)conditionGroup.Operator));

            foreach (var condition in conditionGroup.Conditions)
            {
                queryParams.Add($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{index}].{QueryFieldConditionPropertiesNames.FieldName}",
                    condition.FieldName);
                queryParams.Add($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{index}].{QueryFieldConditionPropertiesNames.Operator}",
                    ((int)condition.Operator).ToString());
                queryParams.Add($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{index}].{QueryFieldConditionPropertiesNames.DataType}",
                    condition.DataType);
                queryParams.Add($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{index}].{QueryFieldConditionPropertiesNames.IsCollection}",
                    ((bool)condition.IsCollection).ToString().ToLower());

                string value = null;
                if (condition.Value != null)
                {
                    if (condition.IsCollection)
                    {
                        StringBuilder joinString = new StringBuilder();

                        foreach (var item in (ICollection)condition.Value)
                        {
                            if (item != null)
                            {
                                if (joinString.Length > 0)
                                    joinString.Append("~");

                                joinString.Append(item);
                            }
                        }
                        value = joinString.ToString();
                    }
                    else
                    {
                        value = Convert.ToString(condition.Value);
                    }
                }

                queryParams.Add($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{index}].{QueryFieldConditionPropertiesNames.Value}",
                        value);


                index++;
            }

            int indexGroup = 0;
            foreach (var cg in conditionGroup.ConditionGroups)
            {
                var dicInnerGroups = GetConditionGroupParams($"{prefix}.{QueryFieldConditionGroupPropertiesNames.Entity}[{indexGroup}]", cg);
                foreach (var item in dicInnerGroups)
                    queryParams.Add(item.Key, item.Value);

                indexGroup++;
            }

            return queryParams;
        }
    }
}

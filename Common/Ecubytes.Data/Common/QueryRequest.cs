using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ecubytes.Data.Common
{
    public class QueryRequest
    {
        private QueryFieldConditionGroup conditionGroup;
        private List<QueryFieldSort> orderFields;

        public int? PageSize { get; set; }
        public int? Page { get; set; }
        public string SearchValue { get; set; }
        public List<QueryFieldSort> SortFields
        {
            get
            {
                if (orderFields == null)
                    orderFields = new List<QueryFieldSort>();
                return orderFields;
            }
            internal set
            {
                orderFields = value;
            }
        }
        public QueryFieldConditionGroup MainConditionGroup
        {
            get
            {
                if(conditionGroup == null)
                    conditionGroup = new QueryFieldConditionGroup();
                return conditionGroup;
            }            
        }

        public QueryRequest AddFieldSort(string fieldName)
        {
            return AddFieldSort(fieldName, SortOrientation.Ascendent);
        }
        public QueryRequest AddFieldSort(string fieldName, SortOrientation sortOrientation)
        {
            int currentSorIndex = 0;
            if(this.SortFields.Any())
                currentSorIndex = this.SortFields.Max(p => p.SortIndex);
            
            return AddFieldSort(fieldName, sortOrientation, currentSorIndex+1);
        }

        public QueryRequest AddFieldSort(
            string fieldName,
            SortOrientation sortOrientation,
            int sortIndex)
        {
            var sort = QueryFieldSort.BuidQueryFieldSort(fieldName, sortOrientation, sortIndex);
            this.SortFields.Add(sort);
            return this;
        }

        public QueryFieldConditionGroup AddConditionGroup()
        {
            return AddConditionGroup(LogicalOperators.And);
        }

        public QueryFieldConditionGroup AddConditionGroup(LogicalOperators logicalOperator)
        {
            var group = QueryFieldConditionGroup.BuildConditionGroup(logicalOperator);
            this.MainConditionGroup.ConditionGroups.Add(group);
            return group;
        }

        public QueryRequest AddCondition(string fieldName, object value)
        {
            AddCondition(fieldName, value, RelationalOperators.Equal);
            return this;
        }

        public QueryRequest AddCondition(string fieldName, object value, RelationalOperators relationalOperator)
        {
            MainConditionGroup.AddCondition(fieldName, value, relationalOperator);
            return this;
        }

        public QueryRequest SetPage(int? page)
        {
            this.Page = page;
            return this;
        }

        public QueryRequest SetPageSize(int? pageSize)
        {
            this.PageSize = pageSize;
            return this;
        }

        public QueryRequest SetSearchValue(string searchValue)
        {
            this.SearchValue = searchValue;
            return this;
        }

        public static QueryRequest Builder()
        {
            return new QueryRequest();
        }
    }

    // public class QueryRequest<T> : QueryRequest
    // {
    //     public T ExtraData { get; set; }
    // }
}

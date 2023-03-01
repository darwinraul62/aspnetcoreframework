using System;
using System.Linq;

namespace Ecubytes.Data.Common
{
    public class QueryFieldSort
    {
        public string FieldName { get; internal set; }
        public SortOrientation SortOrientation { get; internal set; }
        public int SortIndex { get; internal set; }

        public static QueryFieldSort BuidQueryFieldSort(string fieldName, SortOrientation sortOrientation, int sortIndex)
        {
            QueryFieldSort field = new QueryFieldSort();
            field.FieldName = fieldName;
            field.SortOrientation = sortOrientation;
            field.SortIndex = sortIndex;

            return field;
        }
    }
}

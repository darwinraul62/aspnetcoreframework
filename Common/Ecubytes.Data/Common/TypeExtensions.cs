using System;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        public static PropertyInfo GetPropertyCaseInsensitive(this Type type, string propertyName)
        {
            var list = type.GetProperties();
            return list.FirstOrDefault(p=>p.Name.ToLower() == propertyName.ToLower());
        }
    }
}

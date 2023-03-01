using System;
using Microsoft.Extensions.DependencyInjection;
using Ecubytes.AspNetCore.Mvc.DataTables;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataTableRegisterExtensions
    {
        public static IServiceCollection AddMvcDataTables(this IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterDataTables();
            return serviceCollection;
        }
    }
}

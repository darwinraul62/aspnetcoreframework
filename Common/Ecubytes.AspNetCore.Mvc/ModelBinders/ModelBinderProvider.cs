using System;
using Ecubytes.Data.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecubytes.AspNetCore.Mvc.ModelBinders
{
    internal class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder ModelBinder { get; private set; }
        public ModelBinderProvider() { }
        public ModelBinderProvider(IModelBinder modelBinder) { ModelBinder = modelBinder; }
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (IsBindable(context.Metadata.ModelType))
            {
                if (ModelBinder == null) ModelBinder = new QueryRequestModelBinder();
                return ModelBinder;
            }
            else return null;
        }
        private bool IsBindable(Type type) { return type.Equals(typeof(QueryRequest)); }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace iHRS.Api.Binders
{
    public class BodyAndRouteModelBinder : IModelBinder
    {
        private readonly IModelBinder _bodyBinder;
        private readonly IModelBinder _complexBinder;

        public BodyAndRouteModelBinder(IModelBinder bodyBinder, IModelBinder complexBinder)
        {
            _bodyBinder = bodyBinder;
            _complexBinder = complexBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await _bodyBinder.BindModelAsync(bindingContext);

            if (bindingContext.Result.IsModelSet)
            {
                bindingContext.Model = bindingContext.Result.Model;
            }

            await _complexBinder.BindModelAsync(bindingContext);

            var modelType = bindingContext.ModelMetadata.UnderlyingOrModelType;
            var modelInstance = bindingContext.Result.Model;

            foreach (var routParam in bindingContext.ActionContext.RouteData.Values)
            {
                var paramValue = routParam.Value as string;

                if (Guid.TryParse(paramValue, out var paramGuidValue))
                {
                    var idProperty = modelType.GetProperties()
                        .FirstOrDefault(p => p.Name.Equals(routParam.Key, StringComparison.InvariantCultureIgnoreCase));

                    if (idProperty != null)
                    {
                        idProperty.ForceSetValue(modelInstance, paramGuidValue);
                    }
                }
            }

            bindingContext.Result = ModelBindingResult.Success(modelInstance);
        }
    }
}

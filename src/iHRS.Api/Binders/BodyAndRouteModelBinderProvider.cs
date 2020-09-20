using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace iHRS.Api.Binders
{
    public class BodyAndRouteModelBinderProvider : IModelBinderProvider
    {
        private BodyModelBinderProvider _bodyModelBinderProvider;
        private ComplexTypeModelBinderProvider _complexTypeModelBinderProvider;

        public BodyAndRouteModelBinderProvider(BodyModelBinderProvider bodyModelBinderProvider,
            ComplexTypeModelBinderProvider complexTypeModelBinderProvider)
        {
            _bodyModelBinderProvider = bodyModelBinderProvider;
            _complexTypeModelBinderProvider = complexTypeModelBinderProvider;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var bodyBinder = _bodyModelBinderProvider.GetBinder(context);
            var complexBinder = _complexTypeModelBinderProvider.GetBinder(context);

            return new BodyAndRouteModelBinder(bodyBinder, complexBinder);
        }
    }
}
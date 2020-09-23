using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;

namespace iHRS.Api.Binders
{
    public class BodyAndRouteModelBinderProvider : IModelBinderProvider
    {
        private readonly BodyModelBinderProvider _bodyModelBinderProvider;
        private readonly ComplexObjectModelBinderProvider _complexTypeModelBinderProvider;

        public BodyAndRouteModelBinderProvider(BodyModelBinderProvider bodyModelBinderProvider,
            ComplexObjectModelBinderProvider complexTypeModelBinderProvider)
        {
            _bodyModelBinderProvider = bodyModelBinderProvider ?? throw new ArgumentNullException(nameof(bodyModelBinderProvider));
            _complexTypeModelBinderProvider = complexTypeModelBinderProvider ?? throw new ArgumentNullException(nameof(complexTypeModelBinderProvider));
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var bodyBinder = _bodyModelBinderProvider.GetBinder(context);
            var complexBinder = _complexTypeModelBinderProvider.GetBinder(context);

            if (context.BindingInfo.BindingSource != null && context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Body))
                return new BodyAndRouteModelBinder(bodyBinder, complexBinder);


            return null;
        }
    }
}
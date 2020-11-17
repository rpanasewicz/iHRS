﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Collections.Generic;
using System.Linq;

namespace iHRS.Api.Binders
{
    public static class BodyAndRouteModelBinderProviderSetup
    {
        public static void InsertBodyAndRouteBinding(this IList<IModelBinderProvider> providers)
        {
            var bodyProvider =
                providers.Single(provider => provider.GetType() == typeof(BodyModelBinderProvider)) as
                    BodyModelBinderProvider;
            var complexProvider =
                providers.Single(provider => provider.GetType() == typeof(ComplexObjectModelBinderProvider)) as
                    ComplexObjectModelBinderProvider;

            var bodyAndRouteProvider = new BodyAndRouteModelBinderProvider(bodyProvider, complexProvider);

            providers.Insert(0, bodyAndRouteProvider);
        }
    }
}
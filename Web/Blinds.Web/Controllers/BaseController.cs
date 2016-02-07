namespace Blinds.Web.Controllers
{
    using Infrastructure.Unity;
    using Microsoft.Practices.Unity;
    using Models;
    using System.Web.Mvc;

    public abstract class BaseController : Controller
    {
        protected TModel LoadModel<TModel>() where TModel : IModel
        {
            var model = DependencyResolver.LoadModel<TModel>();
            model.Init();
            return model;
        }

        protected TModel LoadModel<TModel, TData>(TData data) where TModel : IModel<TData>
        {
            var model = DependencyResolver.LoadModel<TModel, TData>();
            model.Init(data);
            return model;
        }

        protected MvcUnityDependencyResolver DependencyResolver
        {
            get
            {
                return System.Web.Mvc.DependencyResolver.Current as MvcUnityDependencyResolver;
            }
        }
    }
}
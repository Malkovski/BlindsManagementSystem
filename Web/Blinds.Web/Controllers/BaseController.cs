namespace Blinds.Web.Controllers
{
    using Infrastructure.Unity;
    using Models;
    using System.Web.Mvc;

    public abstract class BaseController : Controller
    {
        protected MvcUnityDependencyResolver DependencyResolver
        {
            get
            {
                return System.Web.Mvc.DependencyResolver.Current as MvcUnityDependencyResolver;
            }
        }

        protected TModel LoadModel<TModel>()
            where TModel : IModel
        {
            var model = this.DependencyResolver.LoadModel<TModel>();
            model.Init();
            return model;
        }

        protected TModel LoadModel<TModel, TData>(TData data)
            where TModel : IModel<TData>
        {
            var model = this.DependencyResolver.LoadModel<TModel, TData>();
            model.Init(data);
            return model;
        }
    }
}
namespace Blinds.Data.RepoFactory
{
    using System.Web.Mvc;

    public class MvcRepoFactory : IRepoFactory
    {
        public T Get<T>()
            where T : class
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }
}

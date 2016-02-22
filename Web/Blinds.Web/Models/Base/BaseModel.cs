namespace Blinds.Web.Models.Base
{
    using Data.RepoFactory;
    using Infrastructure.Caching;
    using Microsoft.Practices.Unity;

    public abstract class BaseModel
    {
        [Dependency]
        public IRepoFactory RepoFactory { get; set; }

        [Dependency]
        public ICacheService Cache { get; set; }
    }
}
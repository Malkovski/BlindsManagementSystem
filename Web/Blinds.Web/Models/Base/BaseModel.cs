namespace Blinds.Web.Models.Base
{
    using Data.RepoFactory;
    using Microsoft.Practices.Unity;

    public abstract class ManuModel
    {
        [Dependency]
        public IRepoFactory RepoFactory { get; set; }
    }
}
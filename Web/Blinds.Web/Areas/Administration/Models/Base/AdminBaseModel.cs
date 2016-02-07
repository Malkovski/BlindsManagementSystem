namespace Blinds.Web.Areas.Administration.Models.Base
{
    using Blinds.Data.RepoFactory;
    using Microsoft.Practices.Unity;

    public abstract class AdminBaseModel
    {
        [Dependency]
        public IRepoFactory RepoFactory { get; set; }
    }
}
namespace Blinds.Web.Models.Base
{
    using AutoMapper;
    using Data.RepoFactory;
    using Infrastructure.Mapping;
    using Microsoft.Practices.Unity;

    public abstract class MainModel
    {
        [Dependency]
        public IRepoFactory RepoFactory { get; set; }

        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }
    }
}
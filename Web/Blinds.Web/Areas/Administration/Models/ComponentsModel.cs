namespace Blinds.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Web.Models;
    using Common;
    using Contracts;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using Kendo.Mvc.UI;
    public class ComponentsModel : MenuModel, IModel<bool>, IMapFrom<Data.Models.Component>, IHaveCustomMappings, IDeletableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.NameRequireText)]
        [DisplayName(GlobalConstants.NameDislply)]
        [UIHint("SingleLineTemplate")]
        public string Name { get; set; }

        [Required(ErrorMessage = GlobalConstants.QuantityRequireText)]
        [DisplayName(GlobalConstants.QuantityDisplay)]
        [UIHint("LongTemplate")]
        public long Quantity { get; set; }

        [Required(ErrorMessage = GlobalConstants.PriceRequireText)]
        [DisplayName(GlobalConstants.PriceDisplay)]
        [UIHint("DecimalTemplate")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = GlobalConstants.BlindTypeRequireText)]
        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        [UIHint("DropDownTemplate")]
        public int BlindTypeId { get; set; }

        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        public string BlindTypeName { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<SelectListItem> BlindTypes { get; set; }

        public void Init(bool init)
        {
            base.Init();

            if (init)
            {
                this.BlindTypes = this.RepoFactory.Get<BlindTypeRepository>().GetAll()
                         .Select(c => new SelectListItem
                         {
                             Value = c.Id.ToString(),
                             Text = c.Name
                         }).ToList();
            }
        }

        public IEnumerable<ComponentsModel> Get()
        {
            return this.RepoFactory.Get<ComponentRepository>().GetActive()
                .Project()
                .To<ComponentsModel>()
                .ToList();
        }

        public DataSourceResult Save(ComponentsModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<ComponentRepository>();
                var entity = repo.GetById(viewModel.Id);

                var exists = repo.GetIfExists(viewModel.BlindTypeId, viewModel.Name, viewModel.Id);

                if (exists)
                {
                    return new DataSourceResult
                    {
                        Errors = GlobalConstants.ComponentExistsMessage
                    };
                }

                if (entity == null)
                {
                    entity = new Data.Models.Component();
                    repo.Add(entity);
                }

                Mapper.Map(viewModel, entity);
                repo.SaveChanges();
                viewModel.Id = entity.Id;
                return null;
            }
            else
            {
                return base.HandleErrors(modelState);
            }
        }

        public DataSourceResult Delete(ComponentsModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<ComponentRepository>();
                var entity = repo.GetById(viewModel.Id);

                entity.Deleted = true;
                entity.DeletedOn = DateTime.Now;

                repo.SaveChanges();
                return null;
            }
            else
            {
                return base.HandleErrors(modelState);
            }
        }

        // Mappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Data.Models.Component, ComponentsModel>();

            configuration.CreateMap<ComponentsModel, Data.Models.Component>().ReverseMap()
                .ForMember(s => s.BlindTypeName, opt => opt.MapFrom(u => u.BlindType.Name));
        }
    }
}
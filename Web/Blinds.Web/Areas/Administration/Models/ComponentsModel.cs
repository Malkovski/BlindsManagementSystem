namespace Blinds.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using Web.Models;
    using Common;
    using Contracts;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using Kendo.Mvc.UI;
    using System.Data.Entity.Validation;
    using AutoMapper.QueryableExtensions;

    public class ComponentsModel : AdminModel, IModel<bool>, IMapFrom<Data.Models.Component>, IHaveCustomMappings, IDeletableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.NameRequireText)]
        [DisplayName(GlobalConstants.NameDislply)]
        [UIHint("SingleLineTemplate")]
        public string Name { get; set; }

        [Required(ErrorMessage = GlobalConstants.QuantityRequireText)]
        [DisplayName(GlobalConstants.QuantityDisplay)]
        [UIHint("DecimalTemplate")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = GlobalConstants.PriceRequireText)]
        [DisplayName(GlobalConstants.PriceDisplay)]
        [UIHint("DecimalTemplate")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = GlobalConstants.DefaultAmountRequireText)]
        [DisplayName(GlobalConstants.DefaultAmountName)]
        [UIHint("DecimalTemplate")]
        public decimal DefaultAmount { get; set; }

        [DisplayName(GlobalConstants.DefaultAmoutHeigthBasedName)]
        [UIHint("BoolTemplate")]
        public bool HeigthBased { get; set; }

        [DisplayName(GlobalConstants.DefaultAmoutWidthBasedName)]
        [UIHint("BoolTemplate")]
        public bool WidthBased { get; set; }

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
            this.Init();

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
                .Project().To<ComponentsModel>()
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

                try
                {
                    repo.SaveChanges();
                    viewModel.Id = entity.Id;
                    return null;
                }
                catch (DbEntityValidationException e)
                {
                    return new DataSourceResult
                    {
                        Errors = this.HandleDbEntityValidationException(e)
                    };
                }
            }
            else
            {
                return this.HandleErrors(modelState);
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
                return this.HandleErrors(modelState);
            }
        }

        // Mappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Data.Models.Component, ComponentsModel>()
                .ForMember(s => s.BlindTypeName, opt => opt.MapFrom(u => u.BlindType.Name));
        }
    }
}
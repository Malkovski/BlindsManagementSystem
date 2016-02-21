namespace Blinds.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using Blinds.Web.Models;
    using Common;
    using Contracts;
    using Data.Models;
    using Data.Models.Enumerations;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using Kendo.Mvc.UI;
    using System.Data.Entity.Validation;
    using AutoMapper.QueryableExtensions;
    public class RailsModel : AdminModel, IModel<bool>, IMapFrom<Rail>, IHaveCustomMappings, IDeletableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.ColorRequireText)]
        [DisplayName(GlobalConstants.ColorDisplay)]
        [UIHint("EnumTemplate")]
        public Color Color { get; set; }

        [Required(ErrorMessage = GlobalConstants.QuantityRequireText)]
        [DisplayName(GlobalConstants.QuantityDisplay)]
        [UIHint("DecimalTemplate")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = GlobalConstants.PriceRequireText)]
        [Range(0, int.MaxValue, ErrorMessage = GlobalConstants.PriceMinValue)]
        [DisplayName(GlobalConstants.PriceDisplay)]
        [UIHint("DecimalTemplate")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = GlobalConstants.BlindTypeRequireText)]
        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        [UIHint("DropDownTemplate")]
        public int BlindTypeId { get; set; }

        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        public string BlindTypeName { get; set; }

        [DisplayName(GlobalConstants.ColorDisplay)]
        public string ColorName
        {
            get
            {
                return this.Color.GetDescription();
            }
        }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<SelectListItem> BlindTypes { get; set; }

        public IEnumerable<SelectListItem> Colors { get; set; }

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

                this.Colors = Enum.GetValues(typeof(Color)).Cast<Color>().Select(v => new SelectListItem
                {
                    Text = v.GetDescription(),
                    Value = ((int)v).ToString()
                }).ToList();
            }
        }

        public IEnumerable<RailsModel> Get()
        {
            return this.RepoFactory.Get<RailRepository>().GetActive()
                .Project()
                .To<RailsModel>()
                .ToList();
        }

        public DataSourceResult Save(RailsModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<RailRepository>();
                var entity = repo.GetById(viewModel.Id);

                var exists = repo.GetIfExists(viewModel.BlindTypeId, viewModel.Color, viewModel.Id);

                if (exists)
                {
                    return new DataSourceResult
                    {
                        Errors = GlobalConstants.RailExistsMessage
                    };
                }

                if (entity == null)
                {
                    entity = new Rail();
                    repo.Add(entity);
                }

                Mapper.Map(viewModel, entity);

                try
                {
                    repo.SaveChanges();
                    viewModel.Id = entity.Id;
                }
                catch (DbEntityValidationException e)
                {
                    return new DataSourceResult
                    {
                        Errors = this.HandleDbEntityValidationException(e)
                    };
                }

                return null;
            }
            else
            {
                return this.HandleErrors(modelState);
            }
        }

        public DataSourceResult Delete(RailsModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<RailRepository>();
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
            configuration.CreateMap<Rail, RailsModel>()
                .ForMember(s => s.BlindTypeName, opt => opt.MapFrom(u => u.BlindType.Name));
        }
    }
}
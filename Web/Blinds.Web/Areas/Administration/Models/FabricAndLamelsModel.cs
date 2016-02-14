namespace Blinds.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using Common;
    using Contracts;
    using Data.Models;
    using Data.Models.Enumerations;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using Web.Models;
    using Kendo.Mvc.UI;
    using System.Data.Entity.Validation;
    using System.Text;
    public class FabricAndLamelsModel : MenuModel, IMapFrom<FabricAndLamel>, IMapTo<FabricAndLamel>, IHaveCustomMappings, IModel<bool>, IDeletableEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.ColorRequireText)]
        [DisplayName(GlobalConstants.ColorDisplay)]
        [UIHint("EnumTemplate")]
        public Color Color { get; set; }

        [Required(ErrorMessage = GlobalConstants.MaterialRequireText)]
        [DisplayName(GlobalConstants.MaterialDisplay)]
        [UIHint("EnumTemplate")]
        public Material Material { get; set; }

        [Required(ErrorMessage = GlobalConstants.QuantityRequireText)]
        [DisplayName(GlobalConstants.QuantityDisplay)]
        [UIHint("DecimalTemplate")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = GlobalConstants.PriceRequireText)]
        [DisplayName(GlobalConstants.PriceDisplay)]
        [Range(0, int.MaxValue, ErrorMessage = GlobalConstants.PriceMinValue)]
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

        [DisplayName(GlobalConstants.MaterialDisplay)]
        public string MaterialName
        {
            get
            {
                return this.Material.GetDescription();
            }
        }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<SelectListItem> BlindTypes { get; set; }

        public IEnumerable<SelectListItem> Colors { get; set; }

        public IEnumerable<SelectListItem> Materials { get; set; }

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

                this.Colors = Enum.GetValues(typeof(Color)).Cast<Color>().Select(v => new SelectListItem
                {
                    Text = v.GetDescription(),
                    Value = ((int)v).ToString()
                }).ToList();

                this.Materials = Enum.GetValues(typeof(Material)).Cast<Material>().Select(v => new SelectListItem
                {
                    Text = v.GetDescription(),
                    Value = ((int)v).ToString()
                }).ToList();
            }
        }

        public IEnumerable<FabricAndLamelsModel> Get()
        {
            return this.RepoFactory.Get<FabricAndLamelRepository>().GetActive()
                .To<FabricAndLamelsModel>()
                .ToList();
        }

        public DataSourceResult Save(FabricAndLamelsModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<FabricAndLamelRepository>();
                var entity = repo.GetById(viewModel.Id);

                var exists = repo.GetIfExists(viewModel.BlindTypeId, viewModel.Color, viewModel.Material, viewModel.Id);

                if (exists)
                {
                    return new DataSourceResult
                    {
                        Errors = GlobalConstants.FabricAndLamelsExistsMessage
                    };
                }

                if (entity == null)
                {
                    entity = new FabricAndLamel();
                    //repo.Add(entity);
                }

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<FabricAndLamelsModel, FabricAndLamel>();
                });
                var mapper = config.CreateMapper();
                entity = mapper.Map<FabricAndLamelsModel, FabricAndLamel>(viewModel);

                try
                {
                    repo.Add(entity);
                    repo.SaveChanges();
                    viewModel.Id = entity.Id;
                    return null;
                }
                catch (DbEntityValidationException e)
                {
                    StringBuilder builder = new StringBuilder();

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        builder.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            builder.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                    }

                    return new DataSourceResult
                    {
                        Errors = builder
                    };
                }
            }
            else
            {
                return base.HandleErrors(modelState);
            }
        }

        public DataSourceResult Delete(FabricAndLamelsModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<FabricAndLamelRepository>();
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
        public void CreateMappings(IMapperConfiguration configuration)
        {

            configuration.CreateMap<FabricAndLamel, FabricAndLamelsModel>()
                .ForMember(s => s.BlindTypeName, opt => opt.MapFrom(u => u.BlindType.Name));
        }
    }
}
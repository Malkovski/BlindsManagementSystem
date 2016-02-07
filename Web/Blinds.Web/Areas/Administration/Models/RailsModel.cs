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
    using Blinds.Web.Models;
    using Common;
    using Contracts;
    using Data.Models;
    using Data.Models.Enumerations;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using Kendo.Mvc.UI;
    public class RailsModel : MenuModel, IModel<bool>, IMapFrom<Rail>, IHaveCustomMappings, IDeletableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.ColorRequireText)]
        [DisplayName(GlobalConstants.ColorDisplay)]
        [UIHint("EnumTemplate")]
        public Color Color { get; set; }

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
            }
        }

        public IEnumerable<RailsModel> Get()
        {
            return this.RepoFactory.Get<RailRepository>().GetActive()
                .Project()
                .To<RailsModel>()
                .ToList();
        }

        public DataSourceResult Save(RailsModel viewModel)
        {
            var repo = this.RepoFactory.Get<RailRepository>();
            var entity = repo.GetById(viewModel.Id);

            var exists = repo.GetIfExists(viewModel.BlindTypeId, viewModel.Color, viewModel.Id);

            if (exists)
            {
                return new DataSourceResult
                {
                    Errors = "Вече съществува релса за този вид щори с този цвят!"
                };
            }

            if (entity == null)
            {
                entity = new Rail();
                repo.Add(entity);
            }

            Mapper.Map(viewModel, entity);

            repo.SaveChanges();
            viewModel.Id = entity.Id;
            return null;
        }

        public void Delete(RailsModel viewModel)
        {
            var repo = this.RepoFactory.Get<RailRepository>();
            var entity = repo.GetById(viewModel.Id);

            entity.Deleted = true;
            entity.DeletedOn = DateTime.Now;

            repo.SaveChanges();
        }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Rail, RailsModel>();

            configuration.CreateMap<RailsModel, Rail>().ReverseMap()
                .ForMember(s => s.BlindTypeName, opt => opt.MapFrom(u => u.BlindType.Name));
        }
    }
}
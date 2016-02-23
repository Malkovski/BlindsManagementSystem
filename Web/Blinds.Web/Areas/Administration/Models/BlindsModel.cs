namespace Blinds.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common;
    using Contracts;
    using Data.Models;
    using Data.Models.Enumerations;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using Kendo.Mvc.UI;
    using Web.Models;

    public class BlindsModel : AdminModel, IModel<bool>, IMapFrom<Blind>, IHaveCustomMappings, IDeletableEntity
    {
        public int Id { get; set; }

        [Required]
        [DisplayName(GlobalConstants.BlindWidth)]
        [Range(1, 5000)]
        [UIHint("DecimalTemplate")]
        public decimal Width { get; set; }

        [Required]
        [DisplayName(GlobalConstants.BlindHeigth)]
        [Range(1, 6000)]
        [UIHint("DecimalTemplate")]
        public decimal Height { get; set; }

        [Required]
        [DisplayName(GlobalConstants.BlindControlSideText)]
        [UIHint("EnumTemplate")]
        public Control Control { get; set; }

        [Required]
        [DisplayName("Id")]
        [UIHint("IntTemplate")]
        public int OrderId { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsTitle)]
        public string OrderNumber { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsOwner)]
        public string OrderOwner { get; set; }

        [DisplayName(GlobalConstants.ColorDisplay)]
        public string ColorName { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<SelectListItem> Controls { get; set; }

        public void Init(bool init)
        {
            this.Init();

            if (init)
            {
                this.Controls = Enum.GetValues(typeof(Control)).Cast<Control>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();
            }
        }

        public IEnumerable<BlindsModel> Get()
        {
            return this.RepoFactory.Get<BlindRepository>().GetActive()
                .Project()
                .To<BlindsModel>()
                .ToList();
        }

        public DataSourceResult Save(BlindsModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<BlindRepository>();
                var entity = repo.GetById(viewModel.Id);

                if (entity == null)
                {
                    entity = new Blind();
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

        public DataSourceResult Delete(BlindsModel viewModel)
        {
            var repo = this.RepoFactory.Get<BlindRepository>();
            var entity = repo.GetById(viewModel.Id);

            entity.Deleted = true;
            entity.DeletedOn = DateTime.Now;

            repo.SaveChanges();
            return null;
        }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Blind, BlindsModel>()
                .ForMember(x => x.ColorName, opt => opt.MapFrom(z => z.Order.FabricAndLamel.Color.ToString()));

            configuration.CreateMap<Blind, BlindsModel>()
                .ForMember(x => x.OrderNumber, opt => opt.MapFrom(z => z.Order.Number));

            configuration.CreateMap<Blind, BlindsModel>()
                .ForMember(x => x.OrderOwner, opt => opt.MapFrom(z => z.Order.User.UserName));
        }
    }
}
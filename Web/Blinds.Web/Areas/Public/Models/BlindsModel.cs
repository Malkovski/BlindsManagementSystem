namespace Blinds.Web.Areas.Public.Models
{
    using Common;
    using Data.Models;
    using Data.Models.Enumerations;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Web.Models;

    public class BlindsModel : MenuModel, IModel<bool>, IMapFrom<Blind>
    {
        public BlindsModel()
        {
            this.Components = new HashSet<Component>();
        }

        public int Id { get; set; }

        [Required, Range(1, 5000)]
        public decimal Width { get; set; }

        [Required, Range(1, 6000)]
        public decimal Height { get; set; }

        [Required]
        public Control Control { get; set; }

        [Required]
        public Color Color { get; set; }

        [Required]
        public int BlindTypeId { get; set; }

        public virtual BlindType BlindType { get; set; }

        [Required]
        public int RailId { get; set; }

        public virtual Rail Rail { get; set; }

        [Required]
        public int FabricAndLamelId { get; set; }

        public virtual FabricAndLamel FabricAndLamel { get; set; }

        [Required]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public virtual ICollection<Component> Components { get; set; }

        public IEnumerable<SelectListItem> BlindTypes { get; set; }

        public IEnumerable<SelectListItem> Colors { get; set; }

        public IEnumerable<SelectListItem> InstalationTypes { get; set; }

        public void Init(bool init)
        {
            this.Init();

            if (init)
            {
                this.BlindTypes = this.RepoFactory.Get<BlindTypeRepository>().GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                this.Colors = Enum.GetValues(typeof(Color)).Cast<Color>().Select(v => new SelectListItem
                {
                    Text = v.GetDescription(),
                    Value = ((int)v).ToString()
                }).ToList();

                this.InstalationTypes = Enum.GetValues(typeof(InstalationType)).Cast<InstalationType>().Select(v => new SelectListItem
                {
                    Text = v.GetDescription(),
                    Value = ((int)v).ToString()
                }).ToList();
            }
        }
    }
}
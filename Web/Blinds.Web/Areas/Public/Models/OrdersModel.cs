namespace Blinds.Web.Areas.Public.Models
{
    using Data.Models;
    using Infrastructure.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Web.Models;
    using Data.Repositories;
    using System.Web.Mvc;
    using Data.Models.Enumerations;
    using Common;

    public class OrdersModel : MenuModel, IModel<bool>, IMapFrom<Order>
    {
        public OrdersModel()
        {
            this.Blinds = new HashSet<BlindsModel>();
        }

        public int Id { get; set; }

        public string Number { get; set; }

        public Color Color { get; set; }

        public InstalationType InstalationType { get; set; }

        public string ColorName {
            get
            {
                return this.Color.GetDescription();
            }
        }

        public string InstalationName
        {
            get
            {
                return this.InstalationType.GetDescription();
            }
        }

        public DateTime OrderDate { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<BlindsModel> Blinds { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public IEnumerable<SelectListItem> BlindTypes { get; set; }

        public IEnumerable<SelectListItem> Colors { get; set; }

        public IEnumerable<SelectListItem> InstalationTypes { get; set; }

        public void Init(bool init)
        {
            base.Init();

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
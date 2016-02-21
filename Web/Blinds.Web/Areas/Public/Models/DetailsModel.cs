namespace Blinds.Web.Areas.Public.Models
{
    using Data.Repositories;
    using Proxies;
    using Common;
    using Web.Models;
    using System.Linq;
    using Data.Models;
    using System;
    using System.Collections.Generic;

    public class DetailsModel : PublicModel, IModel<int>
    {
        public DetailsProxy ViewModel { get; set; }

        public void Init(int id)
        {
            this.Init();
            this.GetData(id);
        }

        public void GetData(int id)
        {
            var order = this.RepoFactory.Get<OrderRepository>().GetById(id);

            var proxy = new DetailsProxy
            {
                OrderNumber = order.Number,
                ClientName = order.User.UserName,
                BlindType = order.BlindType.Name,
                RailColor = order.Rail.Color.GetDescription(),
                FabricAndLamelMaterial = order.FabricAndLamel.Material.GetDescription(),
                FabricAndMaterialColor = order.FabricAndLamel.Color.GetDescription(),
                ExpeditionDate = order.ExpeditionDate,
                Instalation = order.InstalationType.GetDescription(),
                OrderDate = order.OrderDate,
                Price = order.TotalPrice.ToString(),
                Blinds = this.GetBlinds(order)
            };

            this.ViewModel = proxy;
        }

        private IEnumerable<BlindProxy> GetBlinds(Order order)
        {
            var result = new List<BlindProxy>();
            BlindProxy blindProxy;

            var railPrice = this.RepoFactory.Get<RailRepository>().GetById(order.RailId).Price;
            var materialPrice = this.RepoFactory.Get<FabricAndLamelRepository>().GetById(order.FabricAndLamelId).Price;

            var components = this.RepoFactory.Get<ComponentRepository>().GetByBlindType(order.BlindTypeId).ToList();

            foreach (var blind in order.Blinds)
            {
                blindProxy = new BlindProxy
                {
                    Width = blind.Width,
                    Height = blind.Height,
                    ControlName = blind.Control.GetDescription(),
                    Price = this.GetPrice(blind, railPrice, materialPrice, components)
                };

                result.Add(blindProxy);
            }

            return result;
        }

        private decimal GetPrice(Blind blind, decimal railPrice, decimal materialPrice, List<Component> components)
        {
            var railCost = railPrice * (blind.Width / 1000);
            var materialCost = materialPrice * (blind.Width * blind.Height / 1000000);

            decimal componentCost = 0;
            decimal expence = 0;

            foreach (var component in components)
            {
                if (component.HeigthBased && component.WidthBased)
                {
                    var wide = blind.Width < 1000 ? 1000 : (blind.Width / 1000);
                    expence = (blind.Height * (component.DefaultAmount * wide)) / 1000;
                }
                else if (component.HeigthBased)
                {
                    expence = (component.DefaultAmount * blind.Height) / 1000;
                }
                else if (component.WidthBased)
                {
                    expence = (component.DefaultAmount * blind.Width) / 1000;
                }
                else
                {
                    expence = component.DefaultAmount;
                }

                componentCost += expence * component.Price;
            }

            var total = railCost + materialCost + componentCost;


            return total;
        }
    }
}
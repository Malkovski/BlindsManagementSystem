namespace Blinds.Web.Areas.Public.Proxies
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class DetailsProxy
    {
        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsOwner)]
        public string ClientName { get; set; }

        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        public string BlindType { get; set; }

        [DisplayName(GlobalConstants.OrderRailDisplayText)]
        public string RailColor { get; set; }

        [DisplayName(GlobalConstants.OrderMaterialDisplayText)]
        public string FabricAndLamelMaterial { get; set; }

        [DisplayName(GlobalConstants.OrderColorDisplayText)]
        public string FabricAndMaterialColor { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsInstalation)]
        public string Instalation { get; set; }

        [DisplayName(GlobalConstants.OrdersDetailsExpeditionDate)]
        public DateTime ExpeditionDate { get; set; }

        [DisplayName(GlobalConstants.PriceDisplay)]
        public string Price { get; set; }

        public IEnumerable<BlindProxy> Blinds { get; set; }
    }
}
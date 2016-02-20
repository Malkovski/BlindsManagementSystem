namespace Blinds.Web.Areas.Public.Proxies
{
    using System.Collections.Generic;

    public class OrderProxy
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; }

        public int BlindTypeId { get; set; }

        public int RailColorId { get; set; }

        public int FabricAndLamelColorId { get; set; }

        public int FabricAndLamelMaterialId { get; set; }

        public int InstalationTypeId { get; set; }

        public List<BlindProxy> Blinds { get; set; }
    }
}
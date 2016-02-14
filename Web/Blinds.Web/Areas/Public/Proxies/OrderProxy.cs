using Blinds.Common;
using Blinds.Data.Models;
using Blinds.Data.Models.Enumerations;
using Blinds.Web.Areas.Public.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Blinds.Web.Areas.Public.Proxies
{
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
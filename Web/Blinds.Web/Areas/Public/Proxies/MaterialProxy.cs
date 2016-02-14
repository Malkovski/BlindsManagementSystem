namespace Blinds.Web.Areas.Public.Proxies
{
    using Blinds.Data.Models.Enumerations;
    using Common;

    public class MaterialProxy
    {
        public Material Material { get; set; }

        public string MaterialName
        {
            get
            {
                return this.Material.GetDescription();
            }
        }

        public int Value { get; set; }
    }
}
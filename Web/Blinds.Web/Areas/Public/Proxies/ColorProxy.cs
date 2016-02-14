namespace Blinds.Web.Areas.Public.Proxies
{
    using Blinds.Common;
    using Blinds.Data.Models.Enumerations;


    public class ColorProxy
    {
        public Color Color { get; set; }

        public string ColorName
        {
            get
            {
                return this.Color.GetDescription();
            }
        }

        public int Value { get; set; }
    }
}
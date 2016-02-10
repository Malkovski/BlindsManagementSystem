namespace Blinds.Data.Models
{
    using Contracts;
    using Enumerations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ConsumedMaterials : IDeletableEntity
    {
        public ConsumedMaterials()
        {
            this.ComponentsExpence = new HashSet<ConsumedComponent>();
        }

        public int Id { get; set; }

        public virtual Blind Blind { get; set; }

        public int RailId { get; set; }

        public Color RailColor { get; set; }

        public decimal RailExpence { get; set; }

        public decimal RailCost { get; set; }

        public int FabricAndLamelId { get; set; }

        public Color FabricAndLamelColor { get; set; }

        public decimal FabricAndLamelExpence { get; set; }

        public decimal FabricAndLamelCost { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<ConsumedComponent> ComponentsExpence { get; set; }
    }
}

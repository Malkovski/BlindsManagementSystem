namespace Blinds.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;

    public class Blind : IDeletableEntity
    {
        public Blind()
        {
            this.Components = new HashSet<Component>();
        }

        [Key]
        public int Id { get; set; }

        [Required, Range(1, 5000)]
        public decimal Width { get; set; }

        [Required, Range(1, 6000)]
        public decimal Height { get; set; }

        public virtual BlindType BlindType { get; set; }

        [Required]
        public int BlindTypeId { get; set; }

        public virtual Rail Rail { get; set; }

        [Required]
        public int RailId { get; set; }

        public virtual FabricAndLamel FabricAndLamel { get; set; }

        [Required]
        public int FabricAndLamelId { get; set; }

        public virtual ICollection<Component> Components { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

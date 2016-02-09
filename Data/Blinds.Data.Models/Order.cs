namespace Blinds.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using Common;
    using Enumerations;

    public class Order : IDeletableEntity
    {
        public Order()
        {
            this.Blinds = new HashSet<Blind>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public Color Color { get; set; }

        public InstalationType InstalationType { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = GlobalConstants.PriceMinValue)]
        public decimal Price { get; set; }

        public virtual ICollection<Blind> Blinds { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

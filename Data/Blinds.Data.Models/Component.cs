namespace Blinds.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using Common;
    using System.Collections.Generic;

    public class Component : IDeletableEntity
    {
        public Component()
        {
            this.Blinds = new HashSet<Blind>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = GlobalConstants.ComponentNameMinLength)]
        [MaxLength(150, ErrorMessage = GlobalConstants.ComponentNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = GlobalConstants.PriceMinValue)]
        public decimal Price { get; set; }

        public decimal DefaultAmount { get; set; }

        public bool HeigthBased { get; set; }

        public bool WidthBased { get; set; }

        [Required]
        public int BlindTypeId { get; set; }

        public virtual BlindType BlindType { get; set; }

        public virtual ICollection<Blind> Blinds { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

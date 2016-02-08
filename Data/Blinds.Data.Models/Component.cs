namespace Blinds.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using Common;

    public class Component : IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = GlobalConstants.ComponentNameMinLength)]
        [MaxLength(150, ErrorMessage = GlobalConstants.ComponentNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = GlobalConstants.PriceMinValue)]
        public decimal Price { get; set; }

        public virtual BlindType BlindType { get; set; }

        [Required]
        public int BlindTypeId { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

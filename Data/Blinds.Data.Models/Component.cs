namespace Blinds.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Contracts;

    public class Component : IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Името трябва да бъде поне 3 символа!")]
        [MaxLength(150, ErrorMessage = "Името не може да бъде повече от 150 символа!")]
        public string Name { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        public virtual BlindType BlindType { get; set; }

        [Required]
        public int BlindTypeId { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

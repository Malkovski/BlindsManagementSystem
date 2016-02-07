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
        public string Name { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual BlindType BlindType { get; set; }

        [Required]
        public int BlindTypeId { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

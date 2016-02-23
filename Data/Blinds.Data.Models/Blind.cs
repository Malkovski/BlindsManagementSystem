namespace Blinds.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using Enumerations;

    public class Blind : IDeletableEntity
    {
        public Blind()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5000)]
        public decimal Width { get; set; }

        [Required]
        [Range(1, 6000)]
        public decimal Height { get; set; }

        [Required]
        public Control Control { get; set; }

        [Required]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

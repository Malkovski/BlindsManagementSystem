namespace Blinds.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using System.ComponentModel.DataAnnotations.Schema;
    public class BlindType : IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, Index(IsUnique = true)]
        [MinLength(5, ErrorMessage = "Името трябва да е поне 5 символа!")]
        [MaxLength(100, ErrorMessage = "Името не може да е повече от 100 символа!")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Цената не може да бъде отрицателна!")]
        public string Price { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Описанието трябва да е поне 5 символа!")]
        [MaxLength(1500, ErrorMessage = "Името не може да е повече от 1500 символа!")]
        public string Info { get; set; }

        public byte[] Content { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

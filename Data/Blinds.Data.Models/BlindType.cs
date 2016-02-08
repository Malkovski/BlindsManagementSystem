namespace Blinds.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;

    public class BlindType : IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, Index(IsUnique = true)]
        [MinLength(5, ErrorMessage = GlobalConstants.NameMinLength)]
        [MaxLength(100, ErrorMessage = GlobalConstants.NameMaxLength)]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = GlobalConstants.PriceMinValue)]
        public string Price { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = GlobalConstants.InfoMinLength)]
        [MaxLength(1500, ErrorMessage = GlobalConstants.InfoMaxLength)]
        public string Info { get; set; }

        public byte[] Content { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

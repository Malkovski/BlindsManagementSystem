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

        [Required, Index(IsUnique = true), MinLength(5), MaxLength(100)]
        public string Name { get; set; }

        public string Price { get; set; }

        [Required, MinLength(100)]
        public string Info { get; set; }

        public byte[] Content { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

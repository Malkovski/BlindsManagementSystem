namespace Blinds.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Contracts;

    public class BlindType : IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(5)]
        public string Name { get; set; }

        public string Price { get; set; }

        [Required, MinLength(100)]
        public string Info { get; set; }

        public byte[] Content { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

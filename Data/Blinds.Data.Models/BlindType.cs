namespace Blinds.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;

    public class BlindType : IDeletableEntity
    {
        public BlindType()
        {
            this.Pictures = new HashSet<Picture>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MinLength(5, ErrorMessage = GlobalConstants.NameMinLength)]
        [MaxLength(100, ErrorMessage = GlobalConstants.NameMaxLength)]
        public string Name { get; set; }

        public string Price { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = GlobalConstants.InfoMinLength)]
        [MaxLength(1500, ErrorMessage = GlobalConstants.InfoMaxLength)]
        public string Info { get; set; }

        public byte[] Content { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

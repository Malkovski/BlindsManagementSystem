namespace Blinds.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;

    public class Order : IDeletableEntity
    {
        public Order()
        {
            this.Blinds = new HashSet<Blind>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Blind> Blinds { get; set; }

        public virtual User User { get; set; }

        [Required]
        public int UserId { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

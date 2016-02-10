﻿namespace Blinds.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Blinds.Data.Models.Enumerations;
    using Contracts;
    using Common;

    public class Rail : IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Color Color { get; set; }

        [Required]
        public decimal Quantity { get; set; }

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

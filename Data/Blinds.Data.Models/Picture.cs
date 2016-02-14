namespace Blinds.Data.Models
{
    using System;
    using Blinds.Contracts;

    public class Picture : IDeletableEntity
    {
        public int Id { get; set; }

        public string OriginalFileName { get; set; }

        public int OriginalSize { get; set; }

        public string Extension { get; set; }

        public byte[] Content { get; set; }

        public int BlindTypeId { get; set; }

        public virtual BlindType BlindType { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
       
    }
}

namespace Blinds.Data.Models
{
    using System;
    using Blinds.Contracts;

    public class ConsumedComponent : IDeletableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Expence { get; set; }

        public int ConsumedMaterialsId { get; set; }

        public ConsumedMaterials ConsumedMaterials { get; set; }

        public bool Deleted { get; set; }
        
        public DateTime? DeletedOn { get; set; }
       
    }
}

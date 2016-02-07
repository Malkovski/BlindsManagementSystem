namespace Blinds.Contracts
{
    using System;

    public interface IDeletableEntity
    {
        bool Deleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}

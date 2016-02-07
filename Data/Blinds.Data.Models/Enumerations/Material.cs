namespace Blinds.Data.Models.Enumerations
{
    using System.ComponentModel;

    using Blinds.Common;

    public enum Material
    {
        [Description(GlobalConstants.Plastic)]
        Plastic = 1,
        [Description(GlobalConstants.Aluminium)]
        Aluminium = 2,
        [Description(GlobalConstants.Wood)]
        Wood = 4,
        [Description(GlobalConstants.Screen)]
        Screen = 8,
        [Description(GlobalConstants.Blackout)]
        Blackout = 16,
        [Description(GlobalConstants.Transparent)]
        Transparent = 32
    }
}

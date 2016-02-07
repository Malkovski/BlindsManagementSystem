namespace Blinds.Data.Models.Enumerations
{
    using Common;
    using System.ComponentModel;

    public enum Color
    {
        [Description(GlobalConstants.Red)]
        Red = 1,
        [Description(GlobalConstants.Green)]
        Green = 2,
        [Description(GlobalConstants.Blue)]
        Blue = 4
    }
}

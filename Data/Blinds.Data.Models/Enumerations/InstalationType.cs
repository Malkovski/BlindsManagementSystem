namespace Blinds.Data.Models.Enumerations
{
    using Blinds.Common;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum InstalationType
    {
        [Description(GlobalConstants.GTypeInstall)]
        Gtype,
        [Description(GlobalConstants.CTypeInstall)]
        Ctype,
        [Description(GlobalConstants.SideGuidedTypeInstall)]
        SideGuided,
        [Description(GlobalConstants.BottomFixedTypeInstall)]
        BottomFixed
    }
}

namespace Blinds.Web.App_Start
{
    using System.Web.Mvc;

    class ViewEnginesConfiguration
    {
        internal static void RegisterViewEngines(ViewEngineCollection viewEngineCollection)
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}
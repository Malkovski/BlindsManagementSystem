﻿namespace Blinds.Web.Areas.Public.Models
{
    using System.Linq;

    using Blinds.Web.Models;
    using Common;

    using System.Web.Mvc;

    public class PublicModel : MenuModel
    {
        protected string HandleErrors(ModelStateDictionary modelState)
        {
            var error = GlobalConstants.GeneralDataError;

            foreach (var value in modelState.Values)
            {
                if (value.Errors.Count > 0)
                {
                    error = value.Errors.FirstOrDefault().ErrorMessage;
                    break;
                }
            }

            return error;
        }
    }
}
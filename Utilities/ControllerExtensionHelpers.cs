using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CBMS.Utilities
{
    public static class ControllerExtensionHelpers
    {
        public static string GetErrorMessage(this Controller controller, ModelStateDictionary ms)
        {
            StringBuilder sb = new StringBuilder();
            ms.Values.AsParallel().ForAll(
                    value =>
                        value.Errors.AsParallel().ForAll(
                            error =>
                            {
                                sb.Append(error.ErrorMessage);
                            }
                        )
                );
            return sb.ToString();
        }
    }
}
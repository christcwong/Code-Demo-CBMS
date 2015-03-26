using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace CBMS.Utilities
{
    public static class DbEntityValidationExceptionExtension
    {
        public static string GetEntityValidationErrors(this DbEntityValidationException e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (var item in e.EntityValidationErrors)
            {
                sb.Append("Entity : ");
                sb.Append(item.Entry.Entity.ToString());
                sb.AppendLine();
                sb.Append("Error Value: ");
                sb.Append(item.Entry.CurrentValues);
                sb.AppendLine();


                foreach (var error in item.ValidationErrors)
                {
                    sb.Append(" Property Name : ");
                    sb.Append(error.PropertyName);
                    sb.AppendLine();
                    sb.Append(" Error Message : ");
                    sb.Append(error.ErrorMessage);
                    sb.AppendLine();
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
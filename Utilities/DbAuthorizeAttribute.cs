using CBMS.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CBMS.Utilities
{
    public class DbAuthorizeAttribute : AuthorizeAttribute
    {

        //private bool isAuthorized = false;

        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    var user = httpContext.User.Identity.Name;

        //    //if (!isAuthorized)
        //    //{
        //    //    return false;
        //    //}
        //    return base.AuthorizeCore(httpContext);
        //}

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = filterContext.ActionDescriptor.ActionName;

            // Run the default first.

            // TO DO
            // 0) make sure information are all good.
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                          || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization)
            {
                return;
            }
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // auth failed, redirect to login page
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }


            base.OnAuthorization(filterContext);

            if (filterContext.Result != null)
            {
                // the base has deduced the work.
                return;
            }

            // 1) get the current role of user.
            var userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); // new ApplicationUserManager(new ApplicationUserStore(dbContext));
            var roleManager = filterContext.HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); // new ApplicationRoleManager(new ApplicationRoleStore(dbContext));

            var userName = filterContext.HttpContext.User.Identity.Name;

            var user = userManager.FindByName(userName);
            //var user = userManager.FindByNameAsync(userName).Result;

            if (user.Roles != null && user.Roles.Any())
            {

                foreach (var role in user.Roles)
                {
                    // 2) get the accessibility of role
                    var applicationRole = roleManager.FindById(role.RoleId);
                    // 3) see if the role can access this action.
                    if (applicationRole.Accessiblities.Any(a => a.ControllerName == controllerName && a.MethodName == actionName))
                    {
                        //OK.
                        return;
                    }
                }

            }

            // 4) return failure result

            filterContext.Result = new HttpUnauthorizedResult("Your Role is not authorized to access this page.");

        }
    }
}
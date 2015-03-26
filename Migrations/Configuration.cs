namespace CBMS.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using System.Web.Configuration;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using CBMS.Models;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using CBMS.Utilities;
    using System.Reflection;
    using System.Web.Mvc;

    internal sealed class Configuration : DbMigrationsConfiguration<CBMS.Models.CBMSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CBMS.Models.CBMSDbContext";
        }

        protected override void Seed(CBMS.Models.CBMSDbContext context)
        {
            
            SetupDefaultAdmin(context);
            Console.WriteLine("Default Administrator Setup Completed");
            SetupOtherRoles(context);
            Console.WriteLine("Other Roles Setup Completed");
            SetupAccessiblity(context);
            Console.WriteLine("Accessibility of Roles Setup Completed");
            SetupAdminAccessibility(context);
            Console.WriteLine("Accessibility of Administrator Setup Completed");
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            base.Seed(context);
            Console.WriteLine("Base Seed Method Completed");
        }

        private void SetupDefaultAdmin(CBMS.Models.CBMSDbContext dbContext)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(dbContext));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(dbContext));

            //var userManager = HttpContext.Current
            //    .GetOwinContext().GetUserManager<ApplicationUserManager>();

            //var roleManager = HttpContext.Current
            //    .GetOwinContext().Get<ApplicationRoleManager>();

            string name = WebConfigurationManager.AppSettings["CBMS.CONFIG.DEFAULT.ADMIN.NAME"];
            string password = WebConfigurationManager.AppSettings["CBMS.CONFIG.DEFAULT.ADMIN.PASSWORD"];
            string roleName = WebConfigurationManager.AppSettings["CBMS.CONFIG.DEFAULT.ADMIN.ROLE"];


            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                try
                {
                    role = new ApplicationRole(roleName);
                    //throw new Exception("Role.Id = "+role.Id);
                    var roleresult = roleManager.Create(role);
                }
                catch (DbEntityValidationException e)
                {
                    throw new DbEntityValidationException(e.GetEntityValidationErrors(), e);
                }
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
            dbContext.SaveChanges();
        }

        private void SetupOtherRoles(CBMS.Models.CBMSDbContext dbContext)
        {
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(dbContext));
            var roleNames = new List<String> { "HR Manager", "Warehouse Manager", "Store Manager" };

            foreach (var roleName in roleNames)
            {
                //Create Role  if it does not exist
                var role = roleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new ApplicationRole(roleName);
                    var roleresult = roleManager.Create(role);
                }
            }
            dbContext.SaveChanges();
        }

        private void SetupAccessiblity(CBMSDbContext dbContext)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var controllers = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type)).ToList(); //filter controllers



            foreach (var controller in controllers)
            {
                var controllerName = controller.Name.Substring(0, controller.Name.LastIndexOf("Controller", comparisonType: StringComparison.CurrentCultureIgnoreCase));
                var methodNames = controller.GetMethods()
                        .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) && !method.Name.StartsWith("get_") && !method.Name.StartsWith("set_"))
                        .Select(method => method.Name)
                        .Distinct()
                        .Except(
                            typeof(Controller).GetMethods().Select(method => method.Name).ToList()
                        )
                        .ToList();
                foreach (var methodName in methodNames)
                {
                    if (!dbContext.Accessibilities.Any(a => a.ControllerName == controllerName && a.MethodName == methodName))
                    {
                        dbContext.Accessibilities.Add(new Accessiblity()
                        {
                            ControllerName = controllerName,
                            MethodName = methodName
                        });
                    }
                }
            }
            dbContext.SaveChanges();


        }

        private void SetupAdminAccessibility(CBMSDbContext dbContext)
        {
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(dbContext));
            var adminRole = roleManager.FindByName(WebConfigurationManager.AppSettings["CBMS.CONFIG.DEFAULT.ADMIN.ROLE"]);
            foreach (var item in dbContext.Accessibilities.ToList())
            {
                adminRole.Accessiblities.Add(item);
            }
            roleManager.Update(adminRole);
            dbContext.SaveChanges();
        }

    }
}

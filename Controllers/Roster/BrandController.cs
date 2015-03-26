using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBMS.Models.Roster;
using CBMS.Repositories.Interfaces.Roster;
using CBMS.Repositories.Roster;
using CBMS.Models;
using CBMS.Services.Interfaces.Roster;
using CBMS.Services.Roster;
using System.Configuration;
using System.Web.Configuration;
using System.Threading.Tasks;

namespace CBMS.Controllers.Roster
{
    public class BrandController : Controller
    {

        string brandRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.BRAND"];
        string brandServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.BRAND"];


        // GET: Brand
        public ActionResult Index()
        {
            List<BrandModel> BrandModelList = null;

            using (IBrandRepository brandRepository = (IBrandRepository)Activator.CreateInstance(Type.GetType(brandRepositoryName), new CBMSDbContext()))
            {
                IBrandServices brandServices = (IBrandServices)Activator.CreateInstance(Type.GetType(brandServiceName), this.ModelState, brandRepository);
                BrandModelList = brandServices.GetBrands().ToList();
            }

            return View(BrandModelList);
        }

        // GET: Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        [HttpPost]
        public ActionResult Create(BrandModel brandModel)
        {
            if (ModelState.IsValid)
            {
                using (var dbContext=new CBMSDbContext())
                {
                    IBrandRepository brandRepository = (IBrandRepository)Activator.CreateInstance(Type.GetType(brandRepositoryName), dbContext);
                    IBrandServices brandServices = (IBrandServices)Activator.CreateInstance(Type.GetType(brandServiceName), this.ModelState, brandRepository);
                    brandServices.InsertBrand(brandModel);
                    if (ModelState.IsValid)
                    {
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.StatusMessage+=ModelState.Values.SelectMany(m=>m.Errors).ToString();
                return View();
            }
            else
            {
                ViewBag.StatusMessage += ModelState.Values.SelectMany(m => m.Errors).ToString();
                return View();
            }
        }

        // GET: Brand/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            BrandModel BrandModel = null;

            using (IBrandRepository brandRepository = (IBrandRepository)Activator.CreateInstance(Type.GetType(brandRepositoryName), new CBMSDbContext()))
            {
                IBrandServices brandServices = (IBrandServices)Activator.CreateInstance(Type.GetType(brandServiceName), this.ModelState, brandRepository);
                BrandModel =  brandServices.GetBrandById(id);
            }

            return View(BrandModel);
        }

        // POST: Brand/Edit/5
        [HttpPost]
        public ActionResult Edit(BrandModel brandModel)
        {
            if (ModelState.IsValid)
            {
                using (var dbContext = new CBMSDbContext())
                {
                    IBrandRepository brandRepository = (IBrandRepository)Activator.CreateInstance(Type.GetType(brandRepositoryName), dbContext);
                    IBrandServices brandServices = (IBrandServices)Activator.CreateInstance(Type.GetType(brandServiceName), this.ModelState, brandRepository);
                    brandServices.UpdateBrand(brandModel);
                    if (ModelState.IsValid)
                    {
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.StatusMessage += ModelState.Values.SelectMany(m => m.Errors).ToString();
                return View();
            }
            else
            {
                ViewBag.StatusMessage += ModelState.Values.SelectMany(m => m.Errors).ToString();
                return View();
            }
        }

        // GET: Brand/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Brand/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using CBMS.Models;
using CBMS.Models.Payroll;

using CBMS.Repositories.Interfaces.Payroll;
using CBMS.Services.Interfaces.Payroll;
using CBMS.Services.Interfaces.Roster;
using CBMS.Utilities;
using CBMS.Models.Roster;


namespace CBMS.Controllers.Payroll
{
    public class PayrollProfileController : Controller
    {
        private CBMSDbContext db;
        string payrollRecordServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.PAYROLL.PAYROLLRECORD"];
        string payrollServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.PAYROLL.PAYROLL"];
        string outletServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.OUTLET"];
        string staffServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.STAFF"];



        IOutletServices outletServices;
        IPayrollServices payrollServices;
        IStaffServices staffServices;

        public PayrollProfileController()
        {
            this.db = new CBMSDbContext();
            payrollServices = (IPayrollServices)Activator.CreateInstance(Type.GetType(payrollServiceName), this.ModelState, db);
            outletServices = (IOutletServices)Activator.CreateInstance(Type.GetType(outletServiceName), this.ModelState, db);
            staffServices = (IStaffServices)Activator.CreateInstance(Type.GetType(staffServiceName), this.ModelState, db);
        }

        // GET: PayrollProfile
        public async Task<ActionResult> Index(int? outletId,int? deptId)
        {
            return RedirectToAction("Index", "PayrollRecord", new { outletId = outletId,deptId = deptId });
        }

        // GET: PayrollProfile/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PayrollProfileModel payrollProfileModel = payrollServices.GetPayrollProfileById(id.Value);
            if (payrollProfileModel == null)
            {
                return HttpNotFound();
            }
            return View(payrollProfileModel);
        }

        // GET: PayrollProfile/Create
        public async Task<ActionResult> Create(int? outletId,int? deptId)
        {
            DepartmentModel dept=null;
            if (deptId.HasValue)
            {
                dept = this.outletServices.GetDepartmentById(deptId.Value);
                outletId = dept.OutletId;
            }
            if (!outletId.HasValue)
            {
                return RedirectToAction("Index");
            }
            var outlet = await this.outletServices.GetOutletById(outletId.Value);
            if (outlet == null)
            {
                return RedirectToAction("Index");
            }
            var payrollProfileModel = new PayrollProfileModel()
            {
                Outlet = outlet,
                OutletId = outlet.Id,
                Department = dept,
                DepartmentId = deptId,
                Brand = outlet.Brand,
                BrandId = outlet.BrandId,
                ValidUntil = DateTimeWrapper.Now.AddMonths(1)
            };
            return View(payrollProfileModel);
        }

        // POST: PayrollProfile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SalaryLowerBound,SalaryUpperBound,ValidUntil,OutletId,DepartmentId,BrandId")] PayrollProfileModel payrollProfileModel)
        {
            if (ModelState.IsValid)
            {
                payrollServices.InsertPayrollProfile(payrollProfileModel);

                if (ModelState.IsValid)
                {
                    await payrollServices.SaveChangesAsync();
                    return RedirectToAction("Index", "PayrollRecord", new { outletId = payrollProfileModel.OutletId,deptId=payrollProfileModel.DepartmentId });
                }
            }
            return View(payrollProfileModel);
        }

        // GET: PayrollProfile/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayrollProfileModel payrollProfileModel = payrollServices.GetPayrollProfileById(id.Value);

            if (payrollProfileModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", payrollProfileModel.BrandId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", payrollProfileModel.DepartmentId);
            ViewBag.OutletId = new SelectList(db.Outlets, "Id", "Name", payrollProfileModel.OutletId);
            return View(payrollProfileModel);
        }

        // POST: PayrollProfile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SalaryLowerBound,SalaryUpperBound,ValidUntil,OutletId,DepartmentId,BrandId")] PayrollProfileModel payrollProfileModel)
        {
            if (ModelState.IsValid)
            {
                payrollServices.UpdatePayrollProfile(payrollProfileModel);

                if (ModelState.IsValid)
                {
                    await payrollServices.SaveChangesAsync();
                    return RedirectToAction("Index", "PayrollRecord", new { outletId = payrollProfileModel.OutletId, deptId = payrollProfileModel.DepartmentId });
                }
            }

            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", payrollProfileModel.BrandId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", payrollProfileModel.DepartmentId);
            ViewBag.OutletId = new SelectList(db.Outlets, "Id", "Name", payrollProfileModel.OutletId);
            return View(payrollProfileModel);
        }

        // GET: PayrollProfile/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayrollProfileModel payrollProfileModel = payrollServices.GetPayrollProfileById(id.Value);
            if (payrollProfileModel == null)
            {
                return HttpNotFound();
            }
            return View(payrollProfileModel);
        }

        // POST: PayrollProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PayrollProfileModel payrollProfileModel = payrollServices.GetPayrollProfileById(id);
            if (payrollProfileModel == null)
            {
                return HttpNotFound();
            }
            payrollServices.DeletePayrollProfile(payrollProfileModel);
            await payrollServices.SaveChangesAsync();
            return RedirectToAction("Index", "PayrollRecord", new { outletId = payrollProfileModel.OutletId, deptId = payrollProfileModel.DepartmentId });
        }
    }
}

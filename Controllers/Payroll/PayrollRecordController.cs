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
using CBMS.Models.ViewModels;


namespace CBMS.Controllers.Payroll
{
    public class PayrollRecordController : Controller
    {
        private CBMSDbContext db;
        string payrollRecordServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.PAYROLL.PAYROLLRECORD"];
        string payrollServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.PAYROLL.PAYROLL"];
        string outletServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.OUTLET"];
        string staffServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.STAFF"];

        IOutletServices outletServices;
        IPayrollServices payrollServices;
        IPayrollRecordServices payrollRecordServices;
        IStaffServices staffServices;

        /// <summary>
        /// initialize Payroll Record Controller
        /// </summary>
        public PayrollRecordController()
        {
            this.db = new CBMSDbContext();
            payrollServices = (IPayrollServices)Activator.CreateInstance(Type.GetType(payrollServiceName), this.ModelState, db);
            payrollRecordServices = (IPayrollRecordServices)Activator.CreateInstance(Type.GetType(payrollRecordServiceName), this.ModelState, db);
            outletServices = (IOutletServices)Activator.CreateInstance(Type.GetType(outletServiceName), this.ModelState, db);
            staffServices = (IStaffServices)Activator.CreateInstance(Type.GetType(staffServiceName), this.ModelState, db);
        }

        // GET: PayrollRecord
        public async Task<ActionResult> Index(int? outletId,int? deptId)
        {
            if (deptId.HasValue)
            {
                var dept = this.outletServices.GetDepartmentById(deptId.Value);
                if (dept != null)
                {
                    ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name", dept.Outlet.BrandId);
                    ViewBag.OutletIdList = new SelectList(this.outletServices.GetOutlets().Where(o=>o.BrandId==dept.Outlet.BrandId), "Id", "Name", dept.OutletId);
                    ViewBag.DepartmentIdList = new SelectList(this.outletServices.GetDepartments(dept.Outlet), "Id", "Name",deptId.Value);
                    return View(dept.Outlet);
                }
            }
            if (outletId.HasValue)
            {
                var outlet = await this.outletServices.GetOutletById(outletId.Value);
                if (outlet != null)
                {
                    ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name", outlet.BrandId);
                    ViewBag.OutletIdList = new SelectList(this.outletServices.GetOutlets().Where(o=>o.BrandId==outlet.BrandId), "Id", "Name", outlet.Id);
                    ViewBag.DepartmentIdList = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Name");
                    return View(outlet);
                }
            }
            ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name");
            ViewBag.OutletIdList = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Name");
            ViewBag.DepartmentIdList = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Name");
            return View();
        }

        // GET: PayrollRecord/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PayrollRecordModel payrollRecordModel = payrollServices.GetPayrollRecordById(id.Value);
            PayrollRecordViewModel vm = payrollServices.GetPayrollRecordViewModel(payrollRecordModel);
            if (payrollRecordModel == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // GET: PayrollRecord/Create
        public async Task<ActionResult> Create(int? outletId,int? deptId, DateTime? startDate, DateTime? endDate)
        {
            PayrollRecordViewModel payrollViewModel = null;
            if (deptId.HasValue)
            {
                var dept = this.outletServices.GetDepartmentById(deptId.Value);
                if (dept == null)
                {
                    return RedirectToAction("Index");
                }
                payrollViewModel = this.payrollServices.PreparePayrollRecordViewModel(dept);
                payrollViewModel.PayrollRecord.StartDate = startDate.HasValue ? startDate.Value : DateTimeWrapper.StartOfWeek(DateTimeWrapper.Now.AddDays(-7));
                payrollViewModel.PayrollRecord.EndDate = endDate.HasValue ? endDate.Value : DateTimeWrapper.EndOfWeek(DateTimeWrapper.Now.AddDays(-7));
                return View(payrollViewModel);
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
            //var payrollModel = this.payrollServices.PreparePayrollRecord(outlet);
            payrollViewModel = this.payrollServices.PreparePayrollRecordViewModel(outlet);
            payrollViewModel.PayrollRecord.StartDate = startDate.HasValue ? startDate.Value : DateTimeWrapper.StartOfWeek(DateTimeWrapper.Now.AddDays(-7));
            payrollViewModel.PayrollRecord.EndDate = endDate.HasValue ? endDate.Value : DateTimeWrapper.EndOfWeek(DateTimeWrapper.Now.AddDays(-7));
            return View(payrollViewModel);
        }

        // POST: PayrollRecord/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PayrollRecordViewModel vm)
        {
            var payrollRecordModel = vm.PayrollRecord;
            if (ModelState.IsValid)
            {
                payrollServices.SubmitPayrollRecord(payrollRecordModel);

                if (ModelState.IsValid)
                {
                    payrollServices.SaveChanges();
                    return RedirectToAction("Index", new { outletId = payrollRecordModel.OutletId, deptId = payrollRecordModel.DepartmentId });
                }
                ViewBag.StatusMessage = this.GetErrorMessage(this.ModelState);
            }

            payrollRecordModel.Outlet = await this.outletServices.GetOutletById(payrollRecordModel.OutletId);
            foreach (var item in payrollRecordModel.StaffPayrollRecords)
            {
                item.StaffProfile = staffServices.GetStaffProfile(item.StaffProfileId);
            }
            vm.PayrollRecord = payrollRecordModel;

            return View(vm);
        }

        // GET: PayrollRecord/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayrollRecordModel payrollRecordModel = payrollServices.GetPayrollRecordById(id.Value);
            PayrollRecordViewModel vm = payrollServices.GetPayrollRecordViewModel(payrollRecordModel);

            if (payrollRecordModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", payrollRecordModel.DepartmentId);
            ViewBag.OutletId = new SelectList(db.Outlets, "Id", "Name", payrollRecordModel.OutletId);
            return View(vm);
        }

        // POST: PayrollRecord/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PayrollRecordViewModel vm)
        {
            var payrollRecordModel = vm.PayrollRecord;
            if (ModelState.IsValid)
            {
                payrollServices.UpdatePayrollRecord(payrollRecordModel);

                if (ModelState.IsValid)
                {
                    payrollServices.SaveChanges();
                    return RedirectToAction("Index", new { outletId = payrollRecordModel.OutletId,deptId=payrollRecordModel.DepartmentId });
                }
            }


            payrollRecordModel.Outlet = await this.outletServices.GetOutletById(payrollRecordModel.OutletId);
            foreach (var item in payrollRecordModel.StaffPayrollRecords)
            {
                item.StaffProfile = staffServices.GetStaffProfile(item.StaffProfileId);
            }
            vm.PayrollRecord = payrollRecordModel;
            return View(vm);
        }

        // GET: PayrollRecord/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayrollRecordModel payrollRecordModel = payrollServices.GetPayrollRecordById(id.Value);
            PayrollRecordViewModel vm = payrollServices.GetPayrollRecordViewModel(payrollRecordModel);
            if (payrollRecordModel == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // POST: PayrollRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PayrollRecordModel payrollRecordModel = payrollServices.GetPayrollRecordById(id);
            if (payrollRecordModel == null)
            {
                return HttpNotFound();
            }
            payrollServices.DeletePayrollRecord(payrollRecordModel);
            payrollServices.SaveChanges();
            return RedirectToAction("Index", new { outletId = payrollRecordModel.OutletId, deptId = payrollRecordModel.DepartmentId });
        }
    }
}

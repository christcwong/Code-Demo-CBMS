using CBMS.Models;
using CBMS.Services.Interfaces.Config;
using CBMS.Services.Interfaces.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web;
using CBMS.Models.ViewModels;
using CBMS.Utilities;
using CBMS.Models.Roster;
using CBMS.Services.Interfaces.Payroll;
using CBMS.Models.Payroll;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CBMS.Services.Interfaces.Inventory;

namespace CBMS.Controllers
{
    public class AjaxController : Controller
    {
        string staffServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.STAFF"];
        string logServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.STAFFLOG"];
        string outletServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.OUTLET"];
        string brandServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.BRAND"];
        string positionServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.POSITION"];
        string payrollServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.PAYROLL.PAYROLL"];
        string inventoryServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.INVENTORY"];
        string cfgServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.CONFIG.CONFIG"];

        IConfigurationServices cfgServices;
        IBrandServices brandServices;
        IOutletServices outletServices;
        IStaffServices staffServices;
        IStaffLogServices logServices;
        IPositionServices positionServices;
        IPayrollServices payrollServices;
        IInventoryServices inventoryServices;

        private CBMSDbContext db;

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser, string> UserManager { get; set; }

        public AjaxController()
        {
            this.db = new CBMSDbContext();
            this.UserManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(this.db));
            staffServices = (IStaffServices)Activator.CreateInstance(Type.GetType(staffServiceName), this.ModelState, db);
            logServices = (IStaffLogServices)Activator.CreateInstance(Type.GetType(logServiceName), this.ModelState, db);
            outletServices = (IOutletServices)Activator.CreateInstance(Type.GetType(outletServiceName), this.ModelState, db);
            brandServices = (IBrandServices)Activator.CreateInstance(Type.GetType(brandServiceName), this.ModelState, db);
            positionServices = (IPositionServices)Activator.CreateInstance(Type.GetType(positionServiceName), this.ModelState, db);
            payrollServices = (IPayrollServices)Activator.CreateInstance(Type.GetType(payrollServiceName), this.ModelState, db);
            inventoryServices = (IInventoryServices)Activator.CreateInstance(Type.GetType(inventoryServiceName), this.ModelState, db);
            cfgServices = (IConfigurationServices)Activator.CreateInstance(Type.GetType(cfgServiceName), this.ModelState, db);
        }

        #region Get Staff Details of organisation

        public ActionResult ListExpiryingVisaStaff(int? brandId, int? outletId, int? deptId)
        {
            var staffInfoes = this.staffServices.AlertStaffVisaStatus(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.VISA.MONTH"]));
            var staffProfiles = staffInfoes.SelectMany(info => info.StaffProfiles.Where(profile => profile.Status != ObjectStatus.DELETED));
            //return View("List", staffProfiles);
            bool showBrandOutletDepartment = true;
            if (brandId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.BrandId == brandId.Value).ToList();
            }
            if (outletId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.Department.OutletId == outletId.Value).ToList();
            }
            if (deptId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.DepartmentId == deptId.Value).ToList();
            }
            ViewBag.ShowBrandOutletDepartment = showBrandOutletDepartment;
            ViewBag.ShowVisaExpiry = true;
            return PartialView("StaffProfileList", staffProfiles.OrderBy(profile => profile.StaffInfo.Visas.Min(visa => visa.VisaExpiry)));
        }

        public ActionResult ListStaffWithVevoUnChecked(int? brandId, int? outletId, int? deptId)
        {
            var staffInfoes = this.staffServices.AlertStaffVevoStatus();
            var staffProfiles = staffInfoes.SelectMany(info => info.StaffProfiles.Where(profile => profile.Status != ObjectStatus.DELETED));
            //return View("List", staffProfiles);
            bool showBrandOutletDepartment = true;
            if (brandId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.BrandId == brandId.Value).ToList();
            }
            if (outletId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.Department.OutletId == outletId.Value).ToList();
            }
            if (deptId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.DepartmentId == deptId.Value).ToList();
            }
            ViewBag.ShowBrandOutletDepartment = showBrandOutletDepartment;
            return PartialView("StaffProfileList", staffProfiles);
        }

        public ActionResult ListBirthdayStaff(int? brandId, int? outletId, int? deptId)
        {
            var staffInfoes = this.staffServices.AlertStaffBirthday(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.BIRTHDAY.DAY"]));
            var staffProfiles = staffInfoes.SelectMany(info => info.StaffProfiles.Where(profile => profile.Status != ObjectStatus.DELETED));
            //return View("List", staffProfiles);
            bool showBrandOutletDepartment = true;
            if (brandId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.BrandId == brandId.Value).ToList();
            }
            if (outletId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.Department.OutletId == outletId.Value).ToList();
            }
            if (deptId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.DepartmentId == deptId.Value).ToList();
            }
            ViewBag.ShowBrandOutletDepartment = showBrandOutletDepartment;
            ViewBag.ShowBirthday = true;
            return PartialView("StaffProfileList", staffProfiles.OrderBy(profile => profile.StaffInfo.DateOfBirth.Value.Month).ThenBy(profile => profile.StaffInfo.DateOfBirth.Value.Day));
        }

        public ActionResult ListOnLeaveStaff(int? brandId, int? outletId, int? deptId)
        {
            var staffInfoes = this.staffServices.AlertStaffOnLeave();
            var staffProfiles = staffInfoes.SelectMany(info => info.StaffProfiles.Where(profile => profile.Status != ObjectStatus.DELETED));
            //return View("List", staffProfiles);
            bool showBrandOutletDepartment = true;
            if (brandId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.BrandId == brandId.Value).ToList();
            }
            if (outletId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.Department.OutletId == outletId.Value).ToList();
            }
            if (deptId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.DepartmentId == deptId.Value).ToList();
            }
            ViewBag.ShowBrandOutletDepartment = showBrandOutletDepartment;
            return PartialView("StaffProfileList", staffProfiles);
        }

        public ActionResult ListFiredStaff(int? brandId, int? outletId, int? deptId)
        {
            var staffProfiles = this.staffServices.GetFiredStaff();
            //return View("List", staffProfiles);
            bool showBrandOutletDepartment = true;
            if (brandId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.BrandId == brandId.Value).ToList();
            }
            if (outletId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.Department.OutletId == outletId.Value).ToList();
            }
            if (deptId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.DepartmentId == deptId.Value).ToList();
            }
            ViewBag.ShowBrandOutletDepartment = showBrandOutletDepartment;
            return PartialView("StaffProfileList", staffProfiles);
        }



        [HttpPost]
        public ActionResult SearchStaff(string query, int? brandId, int? outletId, int? deptId)
        {
            var staffProfiles = this.staffServices.SearchStaff(query);
            bool showBrandOutletDepartment = true;
            if (brandId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.BrandId == brandId.Value).ToList();
            }
            if (outletId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.Department.OutletId == outletId.Value).ToList();
            }
            if (deptId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.DepartmentId == deptId.Value).ToList();
            }
            ViewBag.ShowBrandOutletDepartment = showBrandOutletDepartment;
            return PartialView("StaffProfileList", staffProfiles);
        }

        [HttpPost]
        public ActionResult ListActiveStaff(int? brandId, int? outletId, int? deptId)
        {
            var staffProfiles = this.staffServices.GetStaffProfiles();
            bool showBrandOutletDepartment = true;
            if (brandId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.BrandId == brandId.Value).ToList();
            }
            if (outletId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.Department.OutletId == outletId.Value).ToList();
            }
            if (deptId.HasValue)
            {
                showBrandOutletDepartment = false;
                staffProfiles = staffProfiles.Where(prof => prof.DepartmentId == deptId.Value).ToList();
            }
            ViewBag.ShowBrandOutletDepartment = showBrandOutletDepartment;
            return PartialView("StaffProfileList", staffProfiles);
        }

        [HttpPost]
        public async Task<ActionResult> GetStaffProfiles(int? brandId, int? outletId, int? deptId, string query)
        {
            if (deptId.HasValue)
            {
                return GetStaffProfilesOfDepartment(deptId, query);
            }
            if (outletId.HasValue)
            {
                return await GetStaffProfilesOfOutlet(outletId, query);
            }
            if (brandId.HasValue)
            {
                return await GetStaffProfilesOfBrand(brandId, query);
            }
            return HttpNotFound();
        }


        [HttpPost]
        public ActionResult GetStaffProfilesOfDepartment(int? deptId, String query)
        {
            StaffProfileListViewModel vm;

            var AlertCount = this.staffServices.AlertStaffVisaStatus(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.VISA.MONTH"]));
            var VevoCount = this.staffServices.AlertStaffVevoStatus();
            var BirthdayCount = this.staffServices.AlertStaffBirthday(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.BIRTHDAY.DAY"]));
            var LeaveCount = this.staffServices.AlertStaffOnLeave();


            if (deptId.HasValue)
            {
                var dept = this.outletServices.GetDepartmentById(deptId.Value);
                var staffProfiles = this.outletServices.GetStaffProfiles(dept);
                AlertCount = AlertCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.DepartmentId == deptId.Value
                                    )
                            ).ToList();
                VevoCount = VevoCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.DepartmentId == deptId.Value
                                    )
                            ).ToList();
                BirthdayCount = BirthdayCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.DepartmentId == deptId.Value
                                    )
                            ).ToList();
                LeaveCount = LeaveCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.DepartmentId == deptId.Value
                                    )
                            ).ToList();

                vm = new StaffProfileListViewModel()
                {
                    DepartmentId = deptId,
                    Department = dept,
                    StaffProfiles = staffProfiles,
                    VisaAlertCount = AlertCount.Count(),
                    VevoAlertCount = VevoCount.Count(),
                    BirthdayCount = BirthdayCount.Count(),
                    LeaveCount = LeaveCount.Count(),
                    query = (!String.IsNullOrEmpty(query)) ? query : null,
                };
            }
            else
            {
                var staffProfiles = staffServices.GetStaffProfiles();
                vm = new StaffProfileListViewModel()
                {
                    StaffProfiles = staffProfiles,
                    VisaAlertCount = AlertCount.Count(),
                    VevoAlertCount = VevoCount.Count(),
                    BirthdayCount = BirthdayCount.Count(),
                    LeaveCount = LeaveCount.Count(),
                    query = (!String.IsNullOrEmpty(query)) ? query : null,
                };
            }
            return PartialView("DepartmentStaffProfileList", vm);
            //var json = Json(this.brandServices.GetOutlets(brand).Select(outlet => new { outlet.Id, outlet.Name }));
            //return json;
            //return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetStaffProfilesOfOutlet(int? outletId, String query)
        {
            StaffProfileListViewModel vm;

            var AlertCount = this.staffServices.AlertStaffVisaStatus(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.VISA.MONTH"]));
            var VevoCount = this.staffServices.AlertStaffVevoStatus();
            var BirthdayCount = this.staffServices.AlertStaffBirthday(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.BIRTHDAY.DAY"]));
            var LeaveCount = this.staffServices.AlertStaffOnLeave();


            if (outletId.HasValue)
            {
                OutletModel outlet = await this.outletServices.GetOutletById(outletId.Value);
                var staffProfiles = this.outletServices.GetStaffProfiles(outlet);
                AlertCount = AlertCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.Department.OutletId == outletId.Value
                                    )
                            ).ToList();
                VevoCount = VevoCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.Department.OutletId == outletId.Value
                                    )
                            ).ToList();
                BirthdayCount = BirthdayCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.Department.OutletId == outletId.Value
                                    )
                            ).ToList();
                LeaveCount = LeaveCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.Department.OutletId == outletId.Value
                                    )
                            ).ToList();

                vm = new StaffProfileListViewModel()
                {
                    OutletId = outletId,
                    Outlet = outlet,
                    StaffProfiles = staffProfiles,
                    VisaAlertCount = AlertCount.Count(),
                    VevoAlertCount = VevoCount.Count(),
                    BirthdayCount = BirthdayCount.Count(),
                    LeaveCount = LeaveCount.Count(),
                    query = (!String.IsNullOrEmpty(query)) ? query : null,
                };
            }
            else
            {
                var staffProfiles = staffServices.GetStaffProfiles();
                vm = new StaffProfileListViewModel()
                {
                    StaffProfiles = staffProfiles,
                    VisaAlertCount = AlertCount.Count(),
                    VevoAlertCount = VevoCount.Count(),
                    BirthdayCount = BirthdayCount.Count(),
                    LeaveCount = LeaveCount.Count(),
                    query = (!String.IsNullOrEmpty(query)) ? query : null,
                };
            }
            return PartialView("OutletStaffProfileList", vm);
            //var json = Json(this.brandServices.GetOutlets(brand).Select(outlet => new { outlet.Id, outlet.Name }));
            //return json;
            //return View();
        }


        [HttpPost]
        public async Task<ActionResult> GetStaffProfilesOfBrand(int? brandId, String query)
        {
            StaffProfileListViewModel vm;

            var AlertCount = this.staffServices.AlertStaffVisaStatus(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.VISA.MONTH"]));
            var VevoCount = this.staffServices.AlertStaffVevoStatus();
            var BirthdayCount = this.staffServices.AlertStaffBirthday(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.BIRTHDAY.DAY"]));
            var LeaveCount = this.staffServices.AlertStaffOnLeave();


            if (brandId.HasValue)
            {
                BrandModel brand = this.brandServices.GetBrandById(brandId.Value);
                var staffProfiles = this.staffServices.GetStaffProfiles()
                                    .Where(prof => prof.BrandId == brandId.Value).ToList();
                AlertCount = AlertCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.BrandId == brandId.Value
                                    )
                            ).ToList();
                VevoCount = VevoCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.BrandId == brandId.Value
                                    )
                            ).ToList();
                BirthdayCount = BirthdayCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.BrandId == brandId.Value
                                    )
                            ).ToList();
                LeaveCount = LeaveCount.Where(
                                info =>
                                    info.StaffProfiles.Any(
                                        prof =>
                                            prof.Status != ObjectStatus.DELETED &&
                                            prof.BrandId == brandId.Value
                                    )
                            ).ToList();

                vm = new StaffProfileListViewModel()
                {
                    BrandId = brandId,
                    Brand = brand,
                    StaffProfiles = staffProfiles,
                    VisaAlertCount = AlertCount.Count(),
                    VevoAlertCount = VevoCount.Count(),
                    BirthdayCount = BirthdayCount.Count(),
                    LeaveCount = LeaveCount.Count(),
                    query = (!String.IsNullOrEmpty(query)) ? query : null,
                };
            }
            else
            {
                var staffProfiles = staffServices.GetStaffProfiles();
                vm = new StaffProfileListViewModel()
                {
                    StaffProfiles = staffProfiles,
                    VisaAlertCount = AlertCount.Count(),
                    VevoAlertCount = VevoCount.Count(),
                    BirthdayCount = BirthdayCount.Count(),
                    LeaveCount = LeaveCount.Count(),
                    query = (!String.IsNullOrEmpty(query)) ? query : null,
                };
            }
            return PartialView("BrandStaffProfileList", vm);
            //var json = Json(this.brandServices.GetOutlets(brand).Select(outlet => new { outlet.Id, outlet.Name }));
            //return json;
            //return View();
        }

        #endregion

        #region GetPayrollRecord


        [HttpPost]
        public async Task<ActionResult> GetPayrollRecords(int? outletId, int? deptId)
        {
            //DepartmentStaffProfileListViewModel vm;

            //var oobOutletCount = this.payrollServices.AlertSalaryOutOfBoundOutlets();
            //var oobRecordCount = this.payrollServices.AlertSalaryOutOfBoundRecords();

            PayrollListViewModel vm;


            if (deptId.HasValue)
            {
                var dept = this.outletServices.GetDepartmentById(deptId.Value);
                var profile = payrollServices.GetLatestPayrollProfile(dept);
                var records = payrollServices.GetPayrollRecords(dept).OrderByDescending(rec => rec.StartDate).ThenByDescending(rec => rec.ObjectUpdateTime).ToList();
                var expiringRecords = this.payrollServices.AlertSalaryOutOfBoundRecords(dept);
                vm = new PayrollListViewModel()
                {
                    Outlet = dept.Outlet,
                    Department = dept,
                    ExpiringProfiles = payrollServices.AlertExpiringPayrollProfile(dept, 14),
                    PayrollProfiles = profile == null ? new List<PayrollProfileModel>() : new List<PayrollProfileModel>() { profile },
                    PayrollRecords = records,
                    PayrollProfileExpiringDictionary =
                        profile == null ?
                        records.ToDictionary(x => x, x => false) :
                        records.ToDictionary(x => x,
                                             x => expiringRecords.Contains(x)),

                };
                return PartialView("PayrollList", vm);
            }


            if (outletId.HasValue)
            {
                var outlet = await this.outletServices.GetOutletById(outletId.Value);
                var profile = payrollServices.GetLatestPayrollProfile(outlet);
                var records = payrollServices.GetPayrollRecords(outlet).OrderByDescending(rec => rec.StartDate).ThenByDescending(rec => rec.ObjectUpdateTime).ToList();
                var expiringRecords = this.payrollServices.AlertSalaryOutOfBoundRecords(outlet);
                vm = new PayrollListViewModel()
                {
                    Outlet = outlet,
                    ExpiringProfiles = payrollServices.AlertExpiringPayrollProfile(outlet, 14),
                    PayrollProfiles = profile == null ? new List<PayrollProfileModel>() : new List<PayrollProfileModel>() { profile },
                    PayrollRecords = records,
                    PayrollProfileExpiringDictionary =
                        profile == null ?
                        records.ToDictionary(x => x, x => false) :
                        records.ToDictionary(x => x,
                                             x => expiringRecords.Contains(x)),
                };
                return PartialView("PayrollList", vm);
            }
            var allRecords = payrollServices.GetPayrollRecords().OrderByDescending(rec => rec.StartDate).ThenByDescending(rec => rec.ObjectUpdateTime).ToList();
            var allExpiringRecords = this.payrollServices.AlertSalaryOutOfBoundRecords();
            vm = new PayrollListViewModel()
            {
                ExpiringProfiles = this.payrollServices.AlertExpiringPayrollProfile(14),
                PayrollProfiles = this.payrollServices.GetPayrollProfiles().OrderByDescending(rec => rec.ValidUntil).ThenByDescending(rec => rec.ObjectUpdateTime).ToList(),
                PayrollRecords = allRecords,
                PayrollProfileExpiringDictionary = allRecords.ToDictionary(x => x, x => allExpiringRecords.Contains(x)),
            };
            return PartialView("PayrollList", vm);
        }

        #endregion

        #region Ajax-call

        [HttpPost]
        public async Task<ActionResult> GetOutletsOfBrand(int brandId)
        {
            var brand = this.brandServices.GetBrandById(brandId);
            var json = Json(this.brandServices.GetOutlets(brand).Select(outlet => new { outlet.Id, outlet.Name }));
            return json;
            //return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetDepartmentsOfOutlet(int outletId)
        {
            var outlet = await this.outletServices.GetOutletById(outletId);
            var json = Json(this.outletServices.GetDepartments(outlet).Select(dept => new { dept.Id, dept.Name }));
            return json;
            //return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetPositionsOfDepartment(int deptId)
        {
            var dept = this.outletServices.GetDepartmentById(deptId);
            var json = Json(this.outletServices.GetPositions(dept).Select(pos => new { pos.Id, pos.FullName }));
            return json;
            //return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetPayCodesOfPosition(int positionId)
        {
            var pos = await this.positionServices.GetPositionById(positionId);
            var json = Json(this.positionServices.GetPayCodes(pos).Select(pc => new { pc.Id, pc.Name }));
            return json;
            //return View();
        }


        [HttpPost]
        public async Task<ActionResult> GetPayCodeValue(int payCodeId)
        {
            var payCode = await this.cfgServices.GetConfigPayCodeValueById(payCodeId);
            var json = Json(new { PayCodeId = payCode.Id, PayCodeValue = payCode.Value });
            return json;
            //return View();
        }


        [HttpPost]
        public async Task<ActionResult> GetPay(int payCodeId, double hours, int staffProfileId)
        {
            var payCode = await this.cfgServices.GetConfigPayCodeValueById(payCodeId);
            var pay = payrollServices.GetPay(payCode, hours, staffProfileId);
            var json = Json(new { PayCodeId = payCode.Id, PayCodeValue = payCode.Value, Pay = pay });
            return json;
            //return View();
        }

        public async Task<ActionResult> GetStaffHistory(int staffProfileId)
        {
            var staffProfile = this.staffServices.GetStaffProfile(staffProfileId);
            if (staffProfile == null)
            {
                return HttpNotFound();
            }
            var changeSets = logServices.ChangesToStaff(staffProfile);
            ViewBag.StaffProfile = staffProfile;
            return PartialView(changeSets);
            //return View();
        }


        public async Task<ActionResult> GetItems(string term)
        {
            var items = this.inventoryServices.GetItems();
            if (!String.IsNullOrEmpty(term))
            {
                items = items.Where(i => i.Serial.StartsWith(term)).ToList();
            }
            var json = Json(items.Select(item => new { Id = item.Id, Serial = item.Serial, Name = item.Name, ChinName = item.ChinName, MYOBLabel = item.MYOBLabel,Unit = item.PackageUnit.Name }), JsonRequestBehavior.AllowGet);
            return json;
            //return View();
        }


        [HttpPost]
        public async Task<ActionResult> UpdateStaffProfilePayCode(int StaffProfileId, int payCodeId)
        {
            var staffProfile = this.staffServices.GetStaffProfile(StaffProfileId);
            var payCode = await this.cfgServices.GetConfigPayCodeValueById(payCodeId);
            if (staffProfile == null || payCode == null)
            {
                return HttpNotFound();
            }

            var oldPayCodeName = staffProfile.PayCode.Name;
            var newPayCodeName = payCode.Name;
            staffProfile = this.staffServices.UpdatePayCode(staffProfile, payCode);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            var json = Json(new { StaffProfileId = staffProfile.Id, OldPayCode = oldPayCodeName, NewPayCodeName = newPayCodeName });
            return json;
            //return View();
        }

        #endregion
    }
}

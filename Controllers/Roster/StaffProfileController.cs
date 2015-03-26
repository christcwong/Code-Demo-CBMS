using CBMS.Models;
using CBMS.Models.Roster;
using CBMS.Models.ViewModels;
using CBMS.Services.Interfaces.Config;
using CBMS.Services.Interfaces.Roster;
using CBMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CBMS.Controllers.Roster
{
    public class StaffProfileController : Controller
    {
        string staffServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.STAFF"];
        string leaveServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.LEAVE"];
        string logServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.STAFFLOG"];
        string outletServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.OUTLET"];
        string brandServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.BRAND"];
        string positionServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.POSITION"];
        string cfgServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.CONFIG.CONFIG"];

        IConfigurationServices cfgServices;
        IBrandServices brandServices;
        IOutletServices outletServices;
        IStaffServices staffServices;
        ILeaveServices leaveServices;
        IStaffLogServices logServices;
        IPositionServices positionServices;

        private CBMSDbContext db;

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser,string> UserManager { get; set; }

        public StaffProfileController()
        {
            this.db = new CBMSDbContext();
            //this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(db));
            staffServices = (IStaffServices)Activator.CreateInstance(Type.GetType(staffServiceName), this.ModelState, db);
            logServices = (IStaffLogServices)Activator.CreateInstance(Type.GetType(logServiceName), this.ModelState, db);
            leaveServices = (ILeaveServices)Activator.CreateInstance(Type.GetType(leaveServiceName), this.ModelState, db);
            outletServices = (IOutletServices)Activator.CreateInstance(Type.GetType(outletServiceName), this.ModelState, db);
            brandServices = (IBrandServices)Activator.CreateInstance(Type.GetType(brandServiceName), this.ModelState, db);
            positionServices = (IPositionServices)Activator.CreateInstance(Type.GetType(positionServiceName), this.ModelState, db);
            cfgServices = (IConfigurationServices)Activator.CreateInstance(Type.GetType(cfgServiceName), this.ModelState, db);
        }
        // GET: StaffProfile
        public async Task<ActionResult> Index(int? brandId, int? outletId, int? deptId, String query)
        {
            ViewBag.AlertCount = this.staffServices.AlertStaffVisaStatus(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.VISA.MONTH"])).Count();
            ViewBag.VevoCount = this.staffServices.AlertStaffVevoStatus().Count();
            ViewBag.BirthdayCount = this.staffServices.AlertStaffBirthday(int.Parse(WebConfigurationManager.AppSettings["CBMS.CONFIG.ALERT.BIRTHDAY.DAY"])).Count();

            ViewBag.Query = String.Empty;
            if (!String.IsNullOrEmpty(query))
            {
                ViewBag.Query = query;
            }
            if (deptId.HasValue)
            {
                var dept = outletServices.GetDepartmentById(deptId.Value);
                if (dept != null)
                {
                    ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name", dept.Outlet.BrandId);
                    ViewBag.OutletIdList = new SelectList(
                                                brandServices.GetOutlets(dept.Outlet.Brand)
                                                .ToList(),
                                                "Id",
                                                "Name",
                                                dept.OutletId
                                                );
                    ViewBag.DepartmentIdList = new SelectList(outletServices.GetDepartments(dept.Outlet), "Id", "Name", dept.Id);
                    return View(new List<StaffProfileModel>());
                }
            }

            if (outletId.HasValue)
            {
                var outlet = await outletServices.GetOutletById(outletId.Value);
                if (outlet != null)
                {
                    ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name", outlet.BrandId);
                    ViewBag.OutletIdList = new SelectList(
                                                brandServices.GetOutlets(outlet.Brand)
                                                .ToList(),
                                                "Id",
                                                "Name",
                                                outlet.Id
                                                );
                    ViewBag.DepartmentIdList = new SelectList(outletServices.GetDepartments(outlet), "Id", "Name");
                    return View(new List<StaffProfileModel>());
                }
            }

            if (brandId.HasValue)
            {
                var brand = brandServices.GetBrandById(brandId.Value);
                if (brand != null)
                {
                    ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name", brand.Id);
                    ViewBag.OutletIdList = new SelectList(
                                                brandServices.GetOutlets(brand)
                                                .ToList(),
                                                "Id",
                                                "Name"
                                                );
                    ViewBag.DepartmentIdList = new SelectList(db.Departments, "Id", "Name");
                    return View(new List<StaffProfileModel>());
                }
            }


            ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name");
            ViewBag.OutletIdList = new SelectList(db.Outlets, "Id", "Name");
            ViewBag.DepartmentIdList = new SelectList(db.Departments, "Id", "Name");
            return View(new List<StaffProfileModel>());
            //return View(staffServices.GetStaffProfiles());
        }


        public async Task<ActionResult> Create(int? brandId, int? outletId, int? deptId)
        {
            ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name");
            ViewBag.OutletIdList = new SelectList(db.Outlets, "Id", "Name");
            ViewBag.DepartmentIdList = new SelectList(db.Departments, "Id", "Name");

            if (brandId.HasValue)
            {
                var brand = this.brandServices.GetBrandById(brandId.Value);
                ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name", brand.Id);
                ;
                ViewBag.OutletIdList = new SelectList(
                                            brandServices.GetOutlets(brand),
                                            "Id",
                                            "Name"
                                            );
                ViewBag.DepartmentIdList = new SelectList(db.Departments, "Id", "Name");
                ViewBag.BrandId = brandId;
            }
            if (outletId.HasValue)
            {
                var outlet = await this.outletServices.GetOutletById(outletId.Value);
                ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name", outlet.BrandId);
                ;
                ViewBag.OutletIdList = new SelectList(
                                            brandServices.GetOutlets(outlet.Brand),
                                            "Id",
                                            "Name",
                                            outletId.Value
                                            );
                ViewBag.DepartmentIdList = new SelectList(outletServices.GetDepartments(outlet), "Id", "Name");
                ViewBag.OutletId = outletId;
            }
            if (deptId.HasValue)
            {
                var dept = this.outletServices.GetDepartmentById(deptId.Value);
                ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name", dept.Outlet.BrandId);
                ;
                ViewBag.OutletIdList = new SelectList(
                                             brandServices.GetOutlets(dept.Outlet.Brand),
                                            "Id",
                                            "Name",
                                            dept.OutletId
                                            );
                ViewBag.DepartmentIdList = new SelectList(outletServices.GetDepartments(dept.Outlet), "Id", "Name", dept.Id);
                ViewBag.DeptId = deptId;
            }
            ViewBag.AdminStatusId = new SelectList(db.StaffProfileAdminStatus, "Id", "LeaveReason");

            ViewBag.PayCodeId = new SelectList(db.ConfigPayCodeValues, "Id", "Name");
            ViewBag.PaymentDetailId = new SelectList(db.StaffProfilePaymentDetails, "Id", "AcName");
            //not working yet, trying to display position with position type
            var PositionList = new SelectList(db.Positions.Select(c => new { Id = c.Id, FullPosition = c.Name + " " + c.ConfigPositionTypeModel.PositionType }), "Id", "FullPosition");
            ViewBag.PositionId = PositionList;
            ViewBag.VisaTypeId = new SelectList(db.ConfigVisaTypes, "Id", "Name");
            ViewBag.ConfigBankModelId = new SelectList(db.ConfigBanks, "Id", "BankName");
            ViewBag.DefaultWorkingHour = 60;
            return View();
        }

        [HttpPost]
        public ActionResult Create(StaffProfileModel staffProfile)
        {
            staffProfile.StaffInfo.Contact.Name = staffProfile.StaffInfo.FullName;

            // try to detach unnecessary job histories if they does not provide so.
            if (staffProfile.StaffInfo.JobHistories.Any())
            {
                var emptyHistory = staffProfile.StaffInfo.JobHistories.Where(hist => String.IsNullOrEmpty(hist.Position) && String.IsNullOrEmpty(hist.CompanyName) && String.IsNullOrEmpty(hist.ReasonOfLeaving));
                foreach (var item in emptyHistory.ToList())
                {
                    staffProfile.StaffInfo.JobHistories.Remove(item);
                }
            }

            //ModelState.Clear();
            //TryValidateModel(staffProfile);
            //ValidateModel(staffProfile);

            if (ModelState.IsValid)
            {
                //staffServices.BeginTransaction();
                staffServices.SubmitApplicationForm(staffProfile);
                if (ModelState.IsValid)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    staffServices.SaveChanges(user);
                    staffServices.Commit();
                    return RedirectToAction("Index", new { deptId = staffProfile.DepartmentId });
                    //ViewBag.StatusMessage = "It is saved... but I am too lazy to redirect you.";
                    //View(staffProfile);
                }
                else
                {
                    staffServices.Rollback();
                    ViewBag.StatusMessage = this.GetErrorMessage(this.ModelState);
                }
            }
            else
            {
                ViewBag.StatusMessage = this.GetErrorMessage(this.ModelState);
            }

            // try to attach unnecessary job histories for display if they does not provide so.
            while (staffProfile.StaffInfo.JobHistories.Count < 2)
            {
                staffProfile.StaffInfo.JobHistories.Add(new StaffInfoJobHistoryModel());
            }

            ViewBag.AdminStatusId = new SelectList(db.StaffProfileAdminStatus, "Id", "LeaveReason", staffProfile.AdminStatusId);
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", staffProfile.BrandId);
            staffProfile.Department = outletServices.GetDepartmentById(staffProfile.DepartmentId.Value);
            ViewBag.OutletIdList = new SelectList(
                                        outletServices.GetOutlets()
                                        .Where(outlet => outlet.BrandId == staffProfile.BrandId)
                                        .ToList(),
                                        "Id",
                                        "Name",
                                        staffProfile.Department.OutletId
                                        );
            ViewBag.DepartmentIdList = new SelectList(outletServices.GetDepartments(staffProfile.Department.Outlet), "Id", "Name", staffProfile.DepartmentId);


            ViewBag.PayCodeId = new SelectList(db.ConfigPayCodeValues, "Id", "Name", staffProfile.PayCodeId);
            ViewBag.PaymentDetailId = new SelectList(db.StaffProfilePaymentDetails, "Id", "AcName", staffProfile.PaymentDetailId);
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name", staffProfile.PositionId);
            //ViewBag.StaffInfoId = new SelectList(db.StaffInfoes, "Id", "FirstName", staffProfile.StaffInfoId);
            ViewBag.VisaTypeId = new SelectList(db.ConfigVisaTypes, "Id", "Name", staffProfile.StaffInfo.Visas.First().Id);
            ViewBag.ConfigBankModelId = new SelectList(db.ConfigBanks, "Id", "BankName", staffProfile.PaymentDetail.ConfigBankModelId);
            ViewBag.DefaultWorkingHour = 60;
            return View(staffProfile);
        }

        /// <summary>
        /// Get the details of staff profile
        /// </summary>
        /// <param name="Id">Id of Staff Profile</param>
        /// <returns></returns>
        public ActionResult Details(int Id)
        {
            var staffProfile = this.staffServices.GetStaffProfile(Id);
            this.leaveServices.UpdateAccumulatedLeaveOfStaff(staffProfile);
            var leaveServiceDummyUserName = "System:Leave Services";
            this.leaveServices.SaveChanges(UserManager.FindByName(leaveServiceDummyUserName) ?? new ApplicationUser() { UserName = leaveServiceDummyUserName });

            return View(staffProfile);
        }


        /// <summary>
        /// Edit Payment Detail
        /// </summary>
        /// <param name="Id">Id of Payment Detail</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int Id)
        {
            var staffProfile = this.staffServices.GetStaffProfile(Id);

            if (staffProfile == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", staffProfile.BrandId);
            staffProfile.Department = outletServices.GetDepartmentById(staffProfile.DepartmentId.Value);
            ViewBag.OutletIdList = new SelectList(
                                        outletServices.GetOutlets()
                                        .Where(outlet => outlet.BrandId == staffProfile.BrandId)
                                        .ToList(),
                                        "Id",
                                        "Name",
                                        staffProfile.Department.OutletId
                                        );
            ViewBag.DepartmentIdList = new SelectList(outletServices.GetDepartments(staffProfile.Department.Outlet), "Id", "Name", staffProfile.DepartmentId);
            ViewBag.PositionIdList = new SelectList(positionServices.GetPositions(staffProfile.Department), "Id", "FullName", staffProfile.PositionId);

            ViewBag.PayCodeIdList = new SelectList((await positionServices.GetPositionById(staffProfile.PositionId.Value)).PayCodes, "Id", "Name", staffProfile.PayCodeId);
            ViewBag.StaffProfileId = Id;
            return View("Edit/StaffProfile", staffProfile);
        }

        [HttpPost]
        public ActionResult Edit(StaffProfileModel staffProfile)
        {
            this.staffServices.EditStaffProfile(staffProfile);
            try
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                staffServices.SaveChanges(user);
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in e.EntityValidationErrors)
                {
                    sb.Append("Entity : ");
                    sb.Append(item.Entry.Entity.ToString());
                    sb.Append("Error Value: ");
                    sb.Append(item.Entry.CurrentValues);


                    foreach (var error in item.ValidationErrors)
                    {
                        sb.Append(" Property Name : ");
                        sb.Append(error.PropertyName);
                        sb.Append(" Error Message : ");
                        sb.Append(error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw new DbEntityValidationException(sb.ToString(), e);
            }
            return RedirectToAction("Details", new { Id = staffProfile.Id });
        }


        /// <summary>
        /// Edit Payment Detail
        /// </summary>
        /// <param name="paymentDetailId">Id of Payment Detail</param>
        /// <returns></returns>
        public ActionResult EditPaymentDetail(int paymentDetailId, int? StaffProfileId)
        {
            var paymentDetail = this.staffServices.GetPaymentDetail(paymentDetailId);

            if (paymentDetail == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ConfigBankModelId = new SelectList(cfgServices.GetConfigBanks().ToList(), "Id", "BankName", paymentDetail.ConfigBankModelId);
            if (StaffProfileId.HasValue)
            {
                ViewBag.StaffProfileId = StaffProfileId.Value;
            }
            return View("Edit/StaffProfilePaymentDetail", paymentDetail);
        }

        [HttpPost]
        public ActionResult EditPaymentDetail(StaffProfilePaymentDetailModel paymentDetail, int? StaffProfileId)
        {
            this.staffServices.UpdatePaymentDetail(paymentDetail);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            if (StaffProfileId.HasValue)
            {
                return RedirectToAction("Details", new { Id = StaffProfileId.Value });
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Edit Admin Status
        /// </summary>
        /// <param name="adminStatusId">Id of Admin Status</param>
        /// <returns></returns>
        public ActionResult EditAdminStatus(int adminStatusId, int? StaffProfileId)
        {
            var AdminStatus = this.staffServices.GetAdminStatus(adminStatusId);

            if (AdminStatus == null)
            {
                return RedirectToAction("Index");
            }
            if (StaffProfileId.HasValue)
            {
                ViewBag.StaffProfileId = StaffProfileId.Value;
            }
            //ViewBag.ConfigBankModelId = new SelectList(cfgBankServices.GetConfigBanks().ToList(), "Id", "BankName", AdminStatus.ConfigBankModelId);
            return View("Edit/StaffProfileAdminStatus", AdminStatus);
        }

        [HttpPost]
        public ActionResult EditAdminStatus(StaffProfileAdminStatusModel AdminStatus, int? StaffProfileId)
        {
            this.staffServices.UpdateAdminStatus(AdminStatus);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            if (StaffProfileId.HasValue)
            {
                return RedirectToAction("Details", new { Id = StaffProfileId.Value });
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Edit Admin Status
        /// </summary>
        /// <param name="LeaveProfileId">Id of Admin Status</param>
        /// <returns></returns>
        public ActionResult EditLeaveProfile(int leaveProfileId, int? StaffProfileId)
        {
            var LeaveProfile = this.staffServices.GetStaffProfileLeaveProfileById(leaveProfileId);

            if (LeaveProfile == null)
            {
                return RedirectToAction("Index");
            }
            if (StaffProfileId.HasValue)
            {
                ViewBag.StaffProfileId = StaffProfileId.Value;
            }
            //ViewBag.ConfigBankModelId = new SelectList(cfgBankServices.GetConfigBanks().ToList(), "Id", "BankName", LeaveProfile.ConfigBankModelId);
            return View("Edit/StaffProfileLeaveProfile", LeaveProfile);
        }

        [HttpPost]
        public ActionResult EditLeaveProfile(StaffProfileLeaveProfileModel LeaveProfile, int? StaffProfileId)
        {
            this.staffServices.UpdateStaffProfileLeaveProfile(LeaveProfile);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            if (StaffProfileId.HasValue)
            {
                return RedirectToAction("Details", new { Id = StaffProfileId.Value });
            }
            return RedirectToAction("Index");
        }



        /// <summary>
        /// Edit Visas
        /// </summary>
        /// <param name="Id">Id of Staff Profile</param>
        /// <returns></returns>
        public ActionResult EditVisas(int Id)
        {
            var staffProfile = this.staffServices.GetStaffProfile(Id);
            var visas = this.staffServices.GetActiveVisa(staffProfile.StaffInfo);

            if (visas == null)
            {
                return RedirectToAction("Index");
            }
            //ViewBag.ConfigBankModelId = new SelectList(cfgBankServices.GetConfigBanks().ToList(), "Id", "BankName", AdminStatus.ConfigBankModelId);
            ViewBag.VisaTypeIdList = new SelectList(cfgServices.GetConfigVisaTypes().ToList(), "Id", "Name");
            ViewBag.StaffProfileId = Id;
            return View("Edit/StaffInfoVisa", new List<StaffInfoVisaModel> { visas });
        }

        [HttpPost]
        public ActionResult EditVisas(IList<StaffInfoVisaModel> visas, int? StaffProfileId)
        {
            foreach (var visa in visas)
            {
                this.staffServices.UpdateVisa(visa);
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            if (StaffProfileId.HasValue)
            {
                return RedirectToAction("Details", new { Id = StaffProfileId.Value });
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Edit Job Histories
        /// </summary>
        /// <param name="adminStatusId">Id of Staff Profile</param>
        /// <returns></returns>
        public ActionResult EditJobHistories(int Id)
        {
            var staffProfile = this.staffServices.GetStaffProfile(Id);
            var JobHistories = this.staffServices.GetJobHistories(staffProfile.StaffInfo);

            if (JobHistories == null || !JobHistories.Any())
            {
                return RedirectToAction("Details", Id);
            }
            //ViewBag.ConfigBankModelId = new SelectList(cfgBankServices.GetConfigBanks().ToList(), "Id", "BankName", AdminStatus.ConfigBankModelId);
            //ViewBag.JobHistoryTypeIdList = new SelectList(cfgServices.GetConfigJobHistoryTypes().ToList(), "Id", "Name");
            return View("Edit/StaffInfoJobHistory", JobHistories);
        }

        [HttpPost]
        public ActionResult EditJobHistories(IList<StaffInfoJobHistoryModel> JobHistories)
        {
            foreach (var JobHistory in JobHistories)
            {
                this.staffServices.UpdateJobHistory(JobHistory);
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Add Leave Record
        /// </summary>
        /// <param name="JobHistoryId">Id of JobHistory</param>
        /// <returns></returns>
        public ActionResult AddJobHistory(int StaffInfoId, int? StaffProfileId)
        {
            var StaffInfo = this.staffServices.GetStaffInfo(StaffInfoId);
            if (StaffInfo == null)
            {
                return RedirectToAction("Index");
            }
            if (StaffProfileId.HasValue)
            {
                ViewBag.StaffProfileId = StaffProfileId.Value;
            }
            return View("AddStaffInfoJobHistory", new StaffInfoJobHistoryModel()
            {
                StaffInfoId = StaffInfoId,
            });
        }

        [HttpPost]
        public ActionResult AddJobHistory(StaffInfoJobHistoryModel JobHistory, int? StaffProfileId)
        {
            var StaffInfo = this.staffServices.GetStaffInfo(JobHistory.StaffInfoId.Value);
            this.staffServices.InsertJobHistory(StaffInfo, JobHistory);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            if (StaffProfileId.HasValue)
            {
                return RedirectToAction("Details", new { Id = StaffProfileId.Value });
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Remove Leave Record
        /// </summary>
        /// <param name="JobHistoryId">Id of JobHistory</param>
        /// <returns></returns>
        //public ActionResult RemoveJobHistory(int StaffInfoId)
        //{
        //    var StaffInfo = this.staffServices.GetStaffInfo(StaffInfoId);
        //    if (StaffInfo == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View("AddStaffInfoJobHistory", new StaffInfoJobHistoryModel() { StaffInfoId = StaffInfoId });
        //}

        //[HttpPost]
        public ActionResult RemoveJobHistory(int StaffInfoId, int JobHistoryId)
        {
            var StaffInfo = this.staffServices.GetStaffInfo(StaffInfoId);
            var JobHistory = this.staffServices.GetJobHistories(StaffInfo).Where(record => record.Id == JobHistoryId).FirstOrDefault();
            JobHistory = this.staffServices.RemoveJobHistory(JobHistory);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            return RedirectToAction("Details", new { Id = StaffInfo.StaffProfiles.First().Id });
        }



        /// <summary>
        /// Edit StaffInfo
        /// </summary>
        /// <param name="StaffInfoId">Id of StaffInfo</param>
        /// <returns></returns>
        public ActionResult EditStaffInfo(int StaffInfoId, int? staffProfileId)
        {
            var StaffInfo = this.staffServices.GetStaffInfo(StaffInfoId);

            if (StaffInfo == null)
            {
                return RedirectToAction("Index");
            }
            //ViewBag.ConfigBankModelId = new SelectList(cfgBankServices.GetConfigBanks().ToList(), "Id", "BankName", StaffInfo.ConfigBankModelId);
            //ViewBag.VisaTypeIdList = new Dictionary<int, SelectList>();
            //foreach (var visa in StaffInfo.Visas)
            //{
            //    ViewBag.VisaTypeIdList[visa.Id] = new SelectList(cfgServices.GetConfigVisaTypes().ToList(), "Id", "Name", visa.Id);
            //}

            //var activeVisa = this.staffServices.GetActiveVisa(StaffInfo);

            //ViewBag.VisaTypeIdList = new SelectList(cfgServices.GetConfigVisaTypes().ToList(), "Id", "Name", activeVisa.Id);

            //StaffInfoEditViewModel vm = new StaffInfoEditViewModel
            //{
            //    ActiveVisa = activeVisa,
            //    StaffInfo = StaffInfo
            //};
            if (staffProfileId.HasValue)
            {
                ViewBag.StaffProfileId = staffProfileId.Value;
            }
            return View("Edit/StaffInfo", StaffInfo);
            //return View("Edit/StaffInfo", vm);
        }

        [HttpPost]
        public ActionResult EditStaffInfo(StaffInfoModel StaffInfo, int? StaffProfileId)
        {
            this.staffServices.EditStaffInfo(StaffInfo);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            if (StaffProfileId.HasValue)
            {
                return RedirectToAction("Details", new { Id = StaffProfileId.Value });
            }
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult EditStaffInfo(StaffProfileDetailViewModel vm)
        //{
        //    var StaffInfo = vm.StaffInfo;
        //    //StaffInfo.Visas.Remove(vm.ActiveVisa);
        //    //StaffInfo.Visas.Add(vm.ActiveVisa);
        //    this.staffServices.EditStaffInfo(StaffInfo);
        //    this.staffServices.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        /// <summary>
        /// Add Leave Record
        /// </summary>
        /// <param name="LeaveRecordId">Id of LeaveRecord</param>
        /// <returns></returns>
        public ActionResult AddLeaveRecord(int StaffProfileId)
        {
            var staffProfile = this.staffServices.GetStaffProfile(StaffProfileId);
            if (staffProfile == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.LeaveTypeList = new SelectList(this.positionServices.GetLeaveTypeForPosition(staffProfile.Position), "Id", "Name");
            return View("AddStaffProfileLeaveRecord", new StaffProfileLeaveRecordModel()
            {
                StaffProfileId = StaffProfileId,
                StartOfLeave = DateTimeWrapper.Now,
                EndOfLeave = DateTimeWrapper.Now,
                DateReturn = DateTimeWrapper.Now.AddDays(1),
                DateOfIssue = DateTimeWrapper.Now,
                DaysOfLeave = 1,
            });
        }

        [HttpPost]
        public ActionResult AddLeaveRecord(StaffProfileLeaveRecordModel LeaveRecord)
        {
            var staffProfile = this.staffServices.GetStaffProfile(LeaveRecord.StaffProfileId.Value);
            this.staffServices.InsertLeaveRecord(staffProfile, LeaveRecord);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            return RedirectToAction("Details", new { Id = LeaveRecord.StaffProfileId.Value });
        }


        /// <summary>
        /// Remove Leave Record
        /// </summary>
        /// <param name="LeaveRecordId">Id of LeaveRecord</param>
        /// <returns></returns>
        //public ActionResult RemoveLeaveRecord(int StaffProfileId)
        //{
        //    var staffProfile = this.staffServices.GetStaffProfile(StaffProfileId);
        //    if (staffProfile == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View("AddStaffProfileLeaveRecord", new StaffProfileLeaveRecordModel() { StaffProfileId = StaffProfileId });
        //}

        //[HttpPost]
        public ActionResult RemoveLeaveRecord(int StaffProfileId, int LeaveRecordId)
        {
            var staffProfile = this.staffServices.GetStaffProfile(StaffProfileId);
            var leaveRecord = this.staffServices.GetLeaveRecords(staffProfile).Where(record => record.Id == LeaveRecordId).FirstOrDefault();
            leaveRecord = this.staffServices.RemoveLeaveRecord(staffProfile, leaveRecord);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            return RedirectToAction("Details", new { Id = StaffProfileId });
        }

        public ActionResult AddStaffProfileToStaff(int StaffInfoId, int? StaffProfileId)
        {
            var staffInfo = this.staffServices.GetStaffInfo(StaffInfoId);
            var StaffProfile = this.staffServices.NewApplicationForm();
            StaffProfile.StaffInfo = staffInfo;
            StaffProfile.StaffInfoId = staffInfo.Id;

            ViewBag.BrandIdList = new SelectList(db.Brands, "Id", "Name");
            ViewBag.OutletIdList = new SelectList(db.Outlets, "Id", "Name");
            ViewBag.DepartmentIdList = new SelectList(db.Departments, "Id", "Name");
            ViewBag.PayCodeId = new SelectList(db.ConfigPayCodeValues, "Id", "Name");
            ViewBag.PaymentDetailId = new SelectList(db.StaffProfilePaymentDetails, "Id", "AcName");
            //not working yet, trying to display position with position type
            var PositionList = new SelectList(db.Positions.Select(c => new { Id = c.Id, FullPosition = c.Name + " " + c.ConfigPositionTypeModel.PositionType }), "Id", "FullPosition");
            ViewBag.PositionId = PositionList;
            ViewBag.VisaTypeId = new SelectList(db.ConfigVisaTypes, "Id", "Name");
            ViewBag.ConfigBankModelId = new SelectList(db.ConfigBanks, "Id", "BankName");

            if (StaffProfileId.HasValue)
            {
                ViewBag.StaffProfileId = StaffProfileId.Value;
            }
            return View("StaffProfileCreate", StaffProfile);
        }

        [HttpPost]
        public ActionResult AddStaffProfileToStaff(StaffProfileModel StaffProfile)
        {
            this.staffServices.BeginTransaction();
            try
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                StaffProfile = this.staffServices.InsertStaffProfile(StaffProfile);
                this.staffServices.SaveChanges(user);
                this.staffServices.AttachStaffProfile(StaffProfile);
                this.staffServices.SaveChanges(user);
                this.staffServices.Commit();
            }
            catch (Exception e)
            {
                this.staffServices.Rollback();
            }
            return RedirectToAction("Details", new { Id = StaffProfile.Id });
        }

        public async Task<ActionResult> Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StaffProfileModel staffProfileModel = this.staffServices.GetStaffProfile(id.Value);
            if (staffProfileModel == null)
            {
                return HttpNotFound();
            }
            return View(staffProfileModel);
        }

        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActivateConfirmed(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StaffProfileModel staffProfileModel = this.staffServices.GetStaffProfile(id);
            if (staffProfileModel == null)
            {
                return HttpNotFound();
            }
            this.staffServices.ActivateStaffProfile(staffProfileModel);
            var user = UserManager.FindById(User.Identity.GetUserId());
            this.staffServices.SaveChanges(user);
            return RedirectToAction("Details", new { Id = id });
        }


        // GET: StaffModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StaffProfileModel staffProfileModel = this.staffServices.GetStaffProfile(id.Value);
            if (staffProfileModel == null)
            {
                return HttpNotFound();
            }
            return View(staffProfileModel);

        }

        // POST: StaffModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StaffProfileModel staffProfileModel = this.staffServices.GetStaffProfile(id);
            if (staffProfileModel == null)
            {
                return HttpNotFound();
            }
            this.staffServices.DeleteStaffProfile(staffProfileModel);
            var user = UserManager.FindById(User.Identity.GetUserId());
            await this.staffServices.SaveChangesAsync(user);
            return RedirectToAction("Index", new { deptId = staffProfileModel.DepartmentId, outletId = staffProfileModel.Department.OutletId });
        }

    }
}
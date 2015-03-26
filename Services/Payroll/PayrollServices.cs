using CBMS.Models;
using CBMS.Models.Config;
using CBMS.Models.Payroll;
using CBMS.Models.Roster;
using CBMS.Models.ViewModels;
using CBMS.Repositories.Interfaces.Config;
using CBMS.Repositories.Interfaces.Payroll;
using CBMS.Repositories.Interfaces.Roster;
using CBMS.Services.Interfaces.Payroll;
using CBMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CBMS.Services.Payroll
{
    public class PayrollServices : CBMSServices, IPayrollServices
    {
        private string PayrollProfileRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.PAYROLL.PAYROLLPROFILE"];
        private string PayrollRecordRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.PAYROLL.PAYROLLRECORD"];
        private string ShiftRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.PAYROLL.SHIFT"];
        private string StaffPayrollRecordRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.PAYROLL.STAFFPAYROLLRECORD"];
        private string staffProfileRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFPROFILE"];
        private string OutletRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.OUTLET"];
        private string payCodeRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGPAYCODEVALUE"];

        private IOutletRepository _outletRepository;
        private IStaffProfileRepository _staffProfileRepository;
        private IStaffPayrollRecordRepository _StaffPayrollRecordRepository;
        private IShiftRepository _ShiftRepository;
        private IPayrollRecordRepository _PayrollRecordRepository;
        private IPayrollProfileRepository _PayrollProfileRepository;
        private IConfigPayCodeValueRepository _payCodeRepository;
        public PayrollServices(ModelStateDictionary modelStateDictionary)
            : this(modelStateDictionary, new CBMSDbContext())
        {
        }
        public PayrollServices(ModelStateDictionary modelStateDictionary, CBMSDbContext dbContext)
        {
            this._modelStateDictionary = modelStateDictionary;
            this.dbContext = dbContext;
            this._PayrollProfileRepository = (IPayrollProfileRepository)Activator.CreateInstance(Type.GetType(PayrollProfileRepositoryName), dbContext);
            this._PayrollRecordRepository = (IPayrollRecordRepository)Activator.CreateInstance(Type.GetType(PayrollRecordRepositoryName), dbContext);
            this._ShiftRepository = (IShiftRepository)Activator.CreateInstance(Type.GetType(ShiftRepositoryName), dbContext);
            this._StaffPayrollRecordRepository = (IStaffPayrollRecordRepository)Activator.CreateInstance(Type.GetType(StaffPayrollRecordRepositoryName), dbContext);
            this._staffProfileRepository = (IStaffProfileRepository)Activator.CreateInstance(Type.GetType(staffProfileRepositoryName), dbContext);
            this._outletRepository = (IOutletRepository)Activator.CreateInstance(Type.GetType(OutletRepositoryName), dbContext);
            this._payCodeRepository = (IConfigPayCodeValueRepository)Activator.CreateInstance(Type.GetType(payCodeRepositoryName), dbContext);
        }


        public PayrollProfileModel InsertPayrollProfile(PayrollProfileModel payrollProfile)
        {
            this._PayrollProfileRepository.Create(payrollProfile);
            return payrollProfile;
        }
        public PayrollProfileModel DeletePayrollProfile(PayrollProfileModel payrollProfile)
        {
            this._PayrollProfileRepository.DeletePayrollProfile(payrollProfile.Id);
            return payrollProfile;
        }
        public PayrollRecordModel DeletePayrollRecord(PayrollRecordModel payrollRecord)
        {
            foreach (var staffRec in payrollRecord.StaffPayrollRecords)
            {
                this._StaffPayrollRecordRepository.DeleteStaffPayrollRecord(staffRec.Id);
            }
            this._PayrollRecordRepository.DeletePayrollRecord(payrollRecord.Id);
            return payrollRecord;
        }
        public PayrollProfileModel GetPayrollProfileById(int payrollProfileId)
        {
            var result = this._PayrollProfileRepository.GetPayrollProfileById(payrollProfileId);
            return result;
        }
        public List<PayrollProfileModel> GetPayrollProfiles()
        {
            var result = this._PayrollProfileRepository
                .GetPayrollProfiles()
                .OrderByDescending(prof => prof.ObjectUpdateTime)
                .ThenByDescending(prof => prof.ValidUntil);
            return result.ToList();
        }
        public List<PayrollProfileModel> GetPayrollProfiles(OutletModel outlet)
        {
            var result = this._PayrollProfileRepository
                .GetPayrollProfiles()
                .Where(prof => prof.OutletId == outlet.Id && !prof.DepartmentId.HasValue)
                .OrderByDescending(prof => prof.ObjectUpdateTime)
                .ThenByDescending(prof => prof.ValidUntil);
            return result.ToList();
        }

        public PayrollProfileModel GetLatestPayrollProfile(OutletModel outlet)
        {
            var result = GetPayrollProfiles(outlet).FirstOrDefault();
            return result;
        }
        public List<PayrollProfileModel> GetPayrollProfiles(DepartmentModel department)
        {
            var result = this._PayrollProfileRepository
                .GetPayrollProfiles()
                .Where(prof => prof.DepartmentId == department.Id)
                .OrderByDescending(prof => prof.ObjectUpdateTime)
                .ThenByDescending(prof => prof.ValidUntil);
            return result.ToList();
        }

        public PayrollProfileModel GetLatestPayrollProfile(DepartmentModel department)
        {
            var result = GetPayrollProfiles(department).FirstOrDefault();
            return result;
        }


        public StaffProfileModel UpdatePayCodeOfStaff(StaffProfileModel staffProfile, ConfigPayCodeValueModel paycode)
        {
            staffProfile.PayCode = paycode;
            this._staffProfileRepository.UpdateStaffProfile(staffProfile);
            return staffProfile;
        }
        public bool ValidateStaffPayrollRecord(StaffPayrollRecordModel staffPayrollRecord)
        {
            bool isValid = true;
            foreach (var result in staffPayrollRecord.Validate(null))
            {
                isValid = false;
                this._modelStateDictionary.AddModelError(result.MemberNames.FirstOrDefault(), result.ErrorMessage);
            }
            return isValid;
        }

        public PayrollRecordModel PreparePayrollRecord(OutletModel outlet)
        {
            var selectedPayrollProfile = GetLatestPayrollProfile(outlet);
            return PreparePayrollRecord(outlet, selectedPayrollProfile);
        }

        public PayrollRecordModel PreparePayrollRecord(OutletModel outlet, PayrollProfileModel payrollProfile)
        {
            var allStaff = this._staffProfileRepository.GetStaffProfiles().Where(staff => staff.Department.OutletId == outlet.Id).ToList();

            var staffPayrollRecordList = allStaff.Select(
                    s =>
                        new StaffPayrollRecordModel()
                        {
                            StaffProfile = s,
                            StaffProfileId = s.Id,
                            Outlet = outlet,
                            OutletId = outlet.Id,
                            Department = s.Department,
                            DepartmentId = s.DepartmentId,
                            PayCode = s.PayCode,
                            PayCodeId = s.PayCodeId.Value,
                            Brand = s.Brand,
                            BrandId = s.BrandId,
                            VoluntaryHolidayPay = s.AdminStatus.VoluntaryHolidayPay,
                            TotalWorkingHours = s.LeaveProfile.WorkingHoursPerWeek
                        }
                ).ToList();

            var payrollRecord = new PayrollRecordModel()
            {
                Outlet = outlet,
                OutletId = outlet.Id,
                Status = ObjectStatus.ACTIVE,
                StaffPayrollRecords = staffPayrollRecordList,
            };
            return payrollRecord;
        }


        public PayrollRecordViewModel PreparePayrollRecordViewModel(OutletModel outlet)
        {
            var selectedPayrollProfile = GetLatestPayrollProfile(outlet);
            return PreparePayrollRecordViewModel(outlet, selectedPayrollProfile);
        }
        public PayrollRecordViewModel PreparePayrollRecordViewModel(OutletModel outlet, PayrollProfileModel payrollProfile)
        {
            var payrollRecord = this.PreparePayrollRecord(outlet, payrollProfile);
            var result = this.GetPayrollRecordViewModel(payrollRecord);

            return result;
        }


        public PayrollRecordModel PreparePayrollRecord(DepartmentModel department)
        {
            var selectedPayrollProfile = GetLatestPayrollProfile(department);
            return PreparePayrollRecord(department, selectedPayrollProfile);
        }

        public PayrollRecordModel PreparePayrollRecord(DepartmentModel department, PayrollProfileModel payrollProfile)
        {
            var allStaff = this._staffProfileRepository.GetStaffProfiles().Where(staff => staff.DepartmentId == department.Id).ToList();

            var staffPayrollRecordList = allStaff.Select(
                    s =>
                        new StaffPayrollRecordModel()
                        {
                            StaffProfile = s,
                            StaffProfileId = s.Id,
                            Outlet = s.Department.Outlet,
                            OutletId = s.Department.OutletId,
                            Department = s.Department,
                            DepartmentId = s.DepartmentId,
                            PayCode = s.PayCode,
                            PayCodeId = s.PayCodeId.Value,
                            Brand = s.Brand,
                            BrandId = s.BrandId,
                            VoluntaryHolidayPay = s.AdminStatus.VoluntaryHolidayPay,
                            TotalWorkingHours = s.LeaveProfile.WorkingHoursPerWeek
                        }
                ).ToList();

            var payrollRecord = new PayrollRecordModel()
            {
                Outlet = department.Outlet,
                OutletId = department.OutletId.Value,
                Department = department,
                DepartmentId = department.Id,
                Status = ObjectStatus.ACTIVE,
                StaffPayrollRecords = staffPayrollRecordList,
            };
            return payrollRecord;
        }


        public PayrollRecordViewModel PreparePayrollRecordViewModel(DepartmentModel department)
        {
            var selectedPayrollProfile = GetLatestPayrollProfile(department);
            return PreparePayrollRecordViewModel(department, selectedPayrollProfile);
        }
        public PayrollRecordViewModel PreparePayrollRecordViewModel(DepartmentModel department, PayrollProfileModel payrollProfile)
        {
            var payrollRecord = this.PreparePayrollRecord(department, payrollProfile);
            var result = this.GetPayrollRecordViewModel(payrollRecord);

            return result;
        }

        public PayrollRecordModel SubmitPayrollRecord(PayrollRecordModel payrollRecord)
        {
            // calculate things....
            // calculate amount for each profile first
            if (payrollRecord.StaffPayrollRecords != null && payrollRecord.StaffPayrollRecords.Any())
            {
                foreach (var rec in payrollRecord.StaffPayrollRecords)
                {
                    if (rec.PayCode == null)
                    {
                        if (rec.PayCodeId != null)
                        {
                            rec.PayCode = this._payCodeRepository.GetConfigPayCodeValueById(rec.PayCodeId);
                        }
                    }
                    var calAmount = GetPay(rec.PayCode, rec.TotalWorkingHours, rec.StaffProfileId);
                    rec.AmountCalculated = calAmount;
                    rec.AmountAdjusted = calAmount;
                    if (rec.Adjustment != 0)
                    {
                        rec.AmountAdjusted += rec.Adjustment;
                    }
                    this._StaffPayrollRecordRepository.InsertStaffPayrollRecord(rec);
                }
                payrollRecord.AmountCalculated = payrollRecord.StaffPayrollRecords.Sum(rec => rec.AmountCalculated);
                payrollRecord.AmountAdjusted = payrollRecord.StaffPayrollRecords.Sum(rec => rec.AmountAdjusted);
            }

            if (payrollRecord.AmountPaid != null)
            {
                //if (payrollRecord.AmountCalculated != payrollRecord.AmountPaid)
                //{
                //    this._modelStateDictionary.AddModelError("Payroll Record Paid", "The paid amount does not match calculated amount");
                //}
                // submitting.
                this._PayrollRecordRepository.InsertPayrollRecord(payrollRecord);
            }
            else
            {
                if (payrollRecord.StaffPayrollRecords.All(rec => rec.AmountPaid != null))
                {
                    payrollRecord.AmountPaid = payrollRecord.StaffPayrollRecords.Sum(rec => rec.AmountPaid);
                    // submitting.
                    this._PayrollRecordRepository.InsertPayrollRecord(payrollRecord);
                }
                else
                {
                    this._modelStateDictionary.AddModelError("Payroll Record Paid", "The paid amount cannot be null.");
                }
            }

            return payrollRecord;
        }
        public List<DepartmentModel> AlertSalaryOutOfBoundDepartments()
        {
            var records = AlertSalaryOutOfBoundRecords();
            var result = records.Select(rec => rec.Department);
            return result.Distinct().ToList();
        }
        public List<OutletModel> AlertSalaryOutOfBoundOutlets()
        {
            var records = AlertSalaryOutOfBoundRecords();
            var result = records.Select(rec => rec.Outlet);
            return result.Distinct().ToList();
        }
        public List<PayrollRecordModel> AlertSalaryOutOfBoundRecords()
        {
            var result = new List<PayrollRecordModel>();

            // foreach outlet, check its active payroll profile.
            var outlets = this._outletRepository.GetOutlets().ToList();

            var profiles = new List<PayrollProfileModel>();
            foreach (var outlet in outlets)
            {
                var profile = this.GetLatestPayrollProfile(outlet);
                if (profile != null)
                {
                    profiles.Add(profile);
                }
            }
            // 2015-01-14 : cannot use parrallel as data reader must be closed first.
            //outlets.AsParallel().ForAll(outlet =>
            //    profiles.Add(this.GetLatestPayrollProfile(outlet))
            //);

            // then check which record is exceeding the bound.
            var payrollRecords = this.GetPayrollRecords();
            foreach (var record in payrollRecords)
            {
                var profile = profiles.Where(prof => prof.OutletId == record.OutletId).First();
                if (record.AmountPaid > profile.SalaryUpperBound || record.AmountPaid < profile.SalaryLowerBound)
                {
                    result.Add(record);
                }
            }
            return result.Distinct().ToList();
        }
        public List<PayrollRecordModel> AlertSalaryOutOfBoundRecords(DepartmentModel department)
        {
            var result = new List<PayrollRecordModel>();
            var profile = this.GetLatestPayrollProfile(department);
            var payrollRecords = this.GetPayrollRecords(department);
            foreach (var record in payrollRecords)
            {
                if (record.AmountPaid > profile.SalaryUpperBound || record.AmountPaid < profile.SalaryLowerBound)
                {
                    result.Add(record);
                }
            }
            return result.Distinct().ToList();
        }
        public List<PayrollRecordModel> AlertSalaryOutOfBoundRecords(OutletModel outlet)
        {
            var result = new List<PayrollRecordModel>();
            var profile = this.GetLatestPayrollProfile(outlet);
            var payrollRecords = this.GetPayrollRecords(outlet);
            if (profile != null)
            {
                foreach (var record in payrollRecords)
                {
                    if (record.AmountPaid > profile.SalaryUpperBound || record.AmountPaid < profile.SalaryLowerBound)
                    {
                        result.Add(record);
                    }
                }
            }
            return result.Distinct().ToList();
        }


        public List<PayrollProfileModel> AlertExpiringPayrollProfile(int days)
        {
            var alertTime = DateTimeWrapper.Now.AddDays(days);
            var payrollProfiles = this._PayrollProfileRepository.GetPayrollProfiles();
            var expiringProfiles = payrollProfiles.Where(prof => prof.ValidUntil <= alertTime);
            return expiringProfiles.ToList();
        }
        public List<PayrollProfileModel> AlertExpiringPayrollProfile(OutletModel outlet, int days)
        {
            var alertTime = DateTimeWrapper.Now.AddDays(days);
            var payrollProfiles = this._PayrollProfileRepository.GetPayrollProfiles().Where(prof => prof.OutletId == outlet.Id);
            var expiringProfiles = payrollProfiles.Where(prof => prof.ValidUntil <= alertTime);
            return expiringProfiles.ToList();
        }
        public List<PayrollProfileModel> AlertExpiringPayrollProfile(DepartmentModel department, int days)
        {
            var alertTime = DateTimeWrapper.Now.AddDays(days);
            var payrollProfiles = this._PayrollProfileRepository.GetPayrollProfiles().Where(prof => prof.DepartmentId == department.Id);
            var expiringProfiles = payrollProfiles.Where(prof => prof.ValidUntil <= alertTime);
            return expiringProfiles.ToList();
        }

        public List<StaffPayrollRecordModel> GetStaffPayrollRecords()
        {
            var result = this._StaffPayrollRecordRepository.GetStaffPayrollRecords();
            return result.ToList();
        }
        public List<StaffPayrollRecordModel> GetStaffPayrollRecords(OutletModel outlet)
        {
            var result = this._StaffPayrollRecordRepository
                .GetStaffPayrollRecords()
                .Where(rec => rec.OutletId == outlet.Id);
            return result.ToList();
        }
        public List<StaffPayrollRecordModel> GetStaffPayrollRecords(DepartmentModel department)
        {
            var result = this._StaffPayrollRecordRepository
                .GetStaffPayrollRecords()
                .Where(rec => rec.DepartmentId == department.Id);
            return result.ToList();
        }
        public StaffPayrollRecordModel GetStaffPayrollRecordById(int StaffPayrollRecordId)
        {
            var result = this._StaffPayrollRecordRepository.GetStaffPayrollRecordById(StaffPayrollRecordId);
            return result;
        }
        public PayrollRecordViewModel GetPayrollRecordViewModel(PayrollRecordModel payrollRecord)
        {
            if (payrollRecord == null)
            {
                return null;
            }

            var staffProfiles = payrollRecord.StaffPayrollRecords.Select(rec => rec.StaffProfile);
            var dictionary = staffProfiles.ToDictionary(x => x, x => x.LeaveProfile.LeaveRecords.Where(lr => lr.Status != ObjectStatus.DELETED).ToList());


            var result = new PayrollRecordViewModel()
            {
                PayrollRecord = payrollRecord,
                staffLeaves = dictionary,
            };

            return result;
        }

        public List<PayrollRecordModel> GetPayrollRecords()
        {
            var result = this._PayrollRecordRepository.GetPayrollRecords();
            return result.ToList();
        }
        public List<PayrollRecordModel> GetPayrollRecords(OutletModel outlet)
        {
            var result = this._PayrollRecordRepository
                .GetPayrollRecords()
                .Where(rec => rec.OutletId == outlet.Id && !rec.DepartmentId.HasValue);
            return result.ToList();
        }
        public List<PayrollRecordModel> GetPayrollRecords(DepartmentModel department)
        {
            var result = this._PayrollRecordRepository
                .GetPayrollRecords()
                .Where(rec => rec.DepartmentId == department.Id);
            return result.ToList();
        }
        public PayrollRecordModel UpdatePayrollRecord(PayrollRecordModel PayrollRecord)
        {

            foreach (var staffRec in PayrollRecord.StaffPayrollRecords)
            {

                //// Since auto mapper duplicate collections in this case, 
                //// I am trying to do a dirty trick :
                //// doing direct db manuipulation here
                //staffRec.ObjectUpdateTime = DateTimeWrapper.Now;
                //dbContext.Entry(staffRec).State = EntityState.Modified;

                // 2015-01-20 : Check Comment below for explaination
                this._StaffPayrollRecordRepository.UpdateStaffPayrollRecord(staffRec);
            }

            //// Since auto mapper duplicate collections in this case, 
            //// I am trying to do a dirty trick :
            //// doing direct db manuipulation here
            //PayrollRecord.ObjectUpdateTime = DateTimeWrapper.Now;
            //dbContext.Entry(PayrollRecord).State = EntityState.Modified;

            // 2015-01-20 : Correct Method : Update children one by one above, and then at parent repository, ignore the changes in client side 
            // Getting children from "database" will result in a lazy loading from the updated version above.
            // so after that the image to be updated will be correct.

            this._PayrollRecordRepository.UpdatePayrollRecord(PayrollRecord);

            // as usual
            return PayrollRecord;
        }

        public PayrollProfileModel UpdatePayrollProfile(PayrollProfileModel PayrollProfile)
        {
            this._PayrollProfileRepository.UpdatePayrollProfile(PayrollProfile);
            return PayrollProfile;
        }

        public PayrollRecordModel GetPayrollRecordById(int PayrollRecordId)
        {
            var result = this._PayrollRecordRepository.GetPayrollRecordById(PayrollRecordId);
            result.StaffPayrollRecords = this._StaffPayrollRecordRepository
                .GetStaffPayrollRecords()
                .Where(rec => rec.PayrollRecordId == PayrollRecordId).ToList();
            return result;
        }

        public double GetPay(ConfigPayCodeValueModel payCode, double hours, int staffProfileId)
        {
            if (payCode.IsHourlyRate)
            {
                return payCode.Value * hours;
            }
            if (payCode.IsWeeklyRate)
            {
                var staffProfile = this._staffProfileRepository.GetStaffProfileById(staffProfileId);
                var storedWorkHours = staffProfile.LeaveProfile.WorkingHoursPerWeek;
                if (storedWorkHours > 0)
                {
                    return Math.Round((payCode.Value / storedWorkHours) * hours, 2, MidpointRounding.AwayFromZero);
                }
            }
            return 0;
        }
    }
}
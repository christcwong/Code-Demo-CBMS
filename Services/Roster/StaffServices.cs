using CBMS.Models;
using CBMS.Models.Config;
using CBMS.Models.Roster;
using CBMS.Repositories.Interfaces.Config;
using CBMS.Repositories.Interfaces.Payroll;
using CBMS.Repositories.Interfaces.Roster;
using CBMS.Services.Interfaces.Roster;
using CBMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CBMS.Services.Roster
{
    public class StaffServices : CBMSServices, IStaffServices
    {
        private string staffProfileRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFPROFILE"];
        private string brandRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.BRAND"];
        private string departmentRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.DEPARTMENT"];
        private string staffInfoRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFINFO"];
        private string staffProfileAdminStatusRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFPROFILEADMINSTATUS"];
        private string staffProfilePaymentDetailRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFPROFILEPAYMENTDETAIL"];
        private string staffProfileLeaveRecordRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFPROFILELEAVERECORD"];
        private string StaffProfileLeaveProfileRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFPROFILELEAVEPROFILE"];
        private string positionRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.POSITION"];
        private string payCodeRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGPAYCODEVALUE"];
        private string staffInfoVisaRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFINFOVISA"];
        private string staffInfoContactRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFINFOCONTACT"];
        private string StaffInfoJobHistoryRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.STAFFINFOJOBHISTORY"];
        private string StaffPayrollRecordRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.PAYROLL.STAFFPAYROLLRECORD"];


        private IStaffProfileLeaveProfileRepository _staffProfileLeaveProfileRepository;
        private IStaffInfoJobHistoryRepository _StaffInfoJobHistoryRepository;
        private IStaffProfileRepository _staffRepository;
        private IBrandRepository _brandRepository;
        private IDepartmentRepository _departmentRepository;
        private IStaffInfoRepository _staffInfoRepository;
        private IStaffInfoContactRepository _staffInfoContactRepository;
        private IStaffInfoVisaRepository _staffInfoVisaRepository;
        private IStaffProfilePaymentDetailRepository _staffProfilePaymentDetailRepository;
        private IStaffProfileAdminStatusRepository _staffProfileAdminStatusRepository;
        private IStaffProfileLeaveRecordRepository _staffProfileLeaveRecordReposiotry;
        private IPositionRepository _positionRepository;
        private IConfigPayCodeValueRepository _payCodeRepository;
        private IStaffPayrollRecordRepository _StaffPayrollRecordRepository;

        public StaffServices(ModelStateDictionary modelStateDictionary)
            : this(modelStateDictionary, new CBMSDbContext())
        {
        }

        public StaffServices(ModelStateDictionary modelStateDictionary, CBMSDbContext dbContext)
        {
            this._modelStateDictionary = modelStateDictionary;
            this.dbContext = dbContext;
            this._staffRepository = (IStaffProfileRepository)Activator.CreateInstance(Type.GetType(staffProfileRepositoryName), dbContext);
            this._brandRepository = (IBrandRepository)Activator.CreateInstance(Type.GetType(brandRepositoryName), dbContext);
            this._departmentRepository = (IDepartmentRepository)Activator.CreateInstance(Type.GetType(departmentRepositoryName), dbContext);
            this._staffInfoRepository = (IStaffInfoRepository)Activator.CreateInstance(Type.GetType(staffInfoRepositoryName), dbContext);
            this._staffInfoContactRepository = (IStaffInfoContactRepository)Activator.CreateInstance(Type.GetType(staffInfoContactRepositoryName), dbContext);
            this._staffInfoVisaRepository = (IStaffInfoVisaRepository)Activator.CreateInstance(Type.GetType(staffInfoVisaRepositoryName), dbContext);
            this._staffProfilePaymentDetailRepository = (IStaffProfilePaymentDetailRepository)Activator.CreateInstance(Type.GetType(staffProfilePaymentDetailRepositoryName), dbContext);
            this._staffProfileAdminStatusRepository = (IStaffProfileAdminStatusRepository)Activator.CreateInstance(Type.GetType(staffProfileAdminStatusRepositoryName), dbContext);
            this._staffProfileLeaveProfileRepository = (IStaffProfileLeaveProfileRepository)Activator.CreateInstance(Type.GetType(StaffProfileLeaveProfileRepositoryName), dbContext);
            this._staffProfileLeaveRecordReposiotry = (IStaffProfileLeaveRecordRepository)Activator.CreateInstance(Type.GetType(staffProfileLeaveRecordRepositoryName), dbContext);
            this._positionRepository = (IPositionRepository)Activator.CreateInstance(Type.GetType(positionRepositoryName), dbContext);
            this._payCodeRepository = (IConfigPayCodeValueRepository)Activator.CreateInstance(Type.GetType(payCodeRepositoryName), dbContext);
            this._StaffInfoJobHistoryRepository = (IStaffInfoJobHistoryRepository)Activator.CreateInstance(Type.GetType(StaffInfoJobHistoryRepositoryName), dbContext);
            this._StaffPayrollRecordRepository = (IStaffPayrollRecordRepository)Activator.CreateInstance(Type.GetType(StaffPayrollRecordRepositoryName), dbContext);
        }

        public StaffProfileModel NewApplicationForm()
        {
            StaffProfileModel model = new StaffProfileModel();
            model.LeaveProfile = new StaffProfileLeaveProfileModel()
            {
                WorkingHoursPerWeek = 60
            };
            return model;
        }
        public bool SubmitApplicationForm(StaffProfileModel staffProfileModel)
        {
            try
            {


                //if (staffProfileModel.BrandId.HasValue)
                //{
                //    var brand = this._brandRepository.Find(staffProfileModel.BrandId.Value);
                //    brand.Staff.Add(staffProfileModel);
                //    this._brandRepository.UpdateBrand(brand);
                //}

                //if (staffProfileModel.DepartmentId.HasValue)
                //{
                //    var department = this._departmentRepository.Find(staffProfileModel.DepartmentId.Value);
                //    department.StaffProfiles.Add(staffProfileModel);
                //    this._departmentRepository.UpdateDepartment(department);
                //}
                //if (staffProfileModel.StaffInfoId.HasValue)
                //{
                //    var staffInfo = this._staffInfoRepository.Find(staffProfileModel.StaffInfoId.Value);
                //    staffInfo.StaffProfiles.Add(staffProfileModel);
                //    this._staffInfoRepository.UpdateStaffInfo(staffInfo);
                //}
                //else
                //{
                //    if (staffProfileModel.StaffInfo != null && staffProfileModel.StaffInfo.Id == 0)
                //    {
                //        _staffInfoRepository.InsertStaffInfo(staffProfileModel.StaffInfo);
                //    }
                //}


                if (staffProfileModel.AdminStatus != null)
                {
                    this._staffProfileAdminStatusRepository.InsertStaffProfileAdminStatus(staffProfileModel.AdminStatus);
                }
                if (staffProfileModel.PaymentDetail != null)
                {
                    this._staffProfilePaymentDetailRepository.InsertStaffProfilePaymentDetail(staffProfileModel.PaymentDetail);
                }
                if (staffProfileModel.LeaveProfile != null)
                {
                    this._staffProfileLeaveProfileRepository.InsertStaffProfileLeaveProfile(staffProfileModel.LeaveProfile);
                    //this.SaveChanges();
                    //staffProfileModel.LeaveProfileId = staffProfileModel.LeaveProfile.Id;
                }
                this._staffRepository.InsertStaffProfile(staffProfileModel);
            }
            catch (Exception ex)
            {
                this._modelStateDictionary.AddModelError("Insert Exception For Submiting Application Form", ex.Message);
                return false;
            }
            return true;
        }

        #region SearchStaff
        public List<StaffProfileModel> SearchStaff(string queryString)
        {
            return SearchStaff(queryString, false);
        }
        public List<StaffProfileModel> SearchStaff(string queryString, bool includeRemovedStaff)
        {
            var potentialStaffs = this._staffRepository.GetStaffProfiles();

            var potentialStaffFilteredByStatus = includeRemovedStaff ?
                this._staffRepository.All() :
                potentialStaffs
                .Where(profile => profile.Brand.Status != ObjectStatus.DELETED)
                .Where(profile => profile.Department.Status != ObjectStatus.DELETED)
                ;

            return potentialStaffFilteredByStatus.Where(
                    profile =>
                        profile.StaffId.Contains(queryString.Trim()) ||
                        profile.RosterName.Contains(queryString.Trim()) ||
                        profile.StaffInfo.FirstName.Contains(queryString.Trim())
                ).ToList();
        }

        public List<StaffProfileModel> SearchStaff(string queryString, BrandModel brand)
        {
            if (brand.Status == ObjectStatus.DELETED)
            {
                return new List<StaffProfileModel>();
            }
            return SearchStaff(queryString).Where(staff => staff.BrandId == brand.Id).ToList();
        }

        public List<StaffProfileModel> SearchStaff(string queryString, OutletModel outlet)
        {
            if (outlet.Status == ObjectStatus.DELETED)
            {
                return new List<StaffProfileModel>();
            }
            return SearchStaff(queryString).Where(staff => staff.Department.Outlet.Id == outlet.Id).ToList();
        }

        public List<StaffProfileModel> SearchStaff(string queryString, DepartmentModel department)
        {
            if (department.Status == ObjectStatus.DELETED)
            {
                return new List<StaffProfileModel>();
            }
            return SearchStaff(queryString).Where(staff => staff.Department.Id == department.Id).ToList();
        }

        public List<StaffProfileModel> SearchStaff(Expression<Func<StaffProfileModel, bool>> predicate)
        {
            var potentialStaffs = this._staffRepository.GetStaffProfiles();
            return potentialStaffs.Where(predicate).ToList();
        }
        #endregion

        public StaffProfileModel GetStaffProfile(int staffProfileId)
        {
            StaffProfileModel staffProfile = this._staffRepository.GetStaffProfileById(staffProfileId);
            staffProfile.StaffInfo = this._staffInfoRepository.GetStaffInfoById(staffProfile.StaffInfoId.Value);
            staffProfile.StaffInfo.Visas = new List<StaffInfoVisaModel>() { this.GetActiveVisa(staffProfile.StaffInfo) };
            staffProfile.StaffInfo.JobHistories = this.GetJobHistories(staffProfile.StaffInfo);
            staffProfile.LeaveProfile.LeaveRecords = this.GetLeaveRecords(staffProfile);
            return staffProfile;
        }

        public List<StaffProfileModel> GetStaffProfiles()
        {
            return this._staffRepository.GetStaffProfiles().ToList();
        }

        public List<StaffProfileModel> GetFiredStaff()
        {
            var staff = this._staffRepository.All();
            var firedStaff = staff.Where(
                    s =>
                        s.Status == ObjectStatus.DELETED
                //s.AdminStatus.LeaveDate.HasValue &&
                //s.AdminStatus.LeaveDate.Value <= DateTimeWrapper.Now
                );
            return firedStaff.ToList();
        }

        public StaffInfoModel GetStaffInfo(StaffProfileModel staffProfile)
        {
            return staffProfile.StaffInfo;
        }
        public StaffInfoModel GetStaffInfo(int StaffInfoId)
        {
            var staffInfo = this._staffInfoRepository.GetStaffInfoById(StaffInfoId);
            return staffInfo;
        }

        public StaffProfileModel InsertStaffProfile(StaffProfileModel staffProfileModel)
        {
            staffProfileModel = this._staffRepository.Create(staffProfileModel);
            return staffProfileModel;
        }

        public StaffProfileModel AttachStaffProfile(StaffProfileModel staffProfileModel)
        {
            try
            {

                if (staffProfileModel.Id == null || staffProfileModel.Id == 0)
                {
                    this._staffRepository.Create(staffProfileModel);
                }
                if (staffProfileModel.BrandId.HasValue)
                {
                    var brand = this._brandRepository.Find(staffProfileModel.BrandId.Value);
                    if (!brand.Staff.Select(s => s.Id).Contains(staffProfileModel.Id))
                    {
                        brand.Staff.Add(staffProfileModel);
                        this._brandRepository.UpdateBrand(brand);
                    }
                }
                if (staffProfileModel.DepartmentId.HasValue)
                {
                    var department = this._departmentRepository.Find(staffProfileModel.DepartmentId);
                    if (!department.StaffProfiles.Select(s => s.Id).Contains(staffProfileModel.Id))
                    {
                        department.StaffProfiles.Add(staffProfileModel);
                        this._departmentRepository.UpdateDepartment(department);
                    }
                }
                if (staffProfileModel.StaffInfoId.HasValue)
                {
                    var staffInfo = this._staffInfoRepository.Find(staffProfileModel.StaffInfoId);
                    if (!staffInfo.StaffProfiles.Select(s => s.Id).Contains(staffProfileModel.Id))
                    {
                        staffInfo.StaffProfiles.Add(staffProfileModel);
                        this._staffInfoRepository.UpdateStaffInfo(staffInfo);
                    }
                }

                if (staffProfileModel.AdminStatus != null)
                {
                    this._staffProfileAdminStatusRepository.InsertStaffProfileAdminStatus(staffProfileModel.AdminStatus);
                }
                if (staffProfileModel.PaymentDetail != null)
                {
                    this._staffProfilePaymentDetailRepository.InsertStaffProfilePaymentDetail(staffProfileModel.PaymentDetail);
                }
                if (staffProfileModel.LeaveProfile != null)
                {
                    this._staffProfileLeaveProfileRepository.InsertStaffProfileLeaveProfile(staffProfileModel.LeaveProfile);
                    //this.SaveChanges();
                    //staffProfileModel.LeaveProfileId = staffProfileModel.LeaveProfile.Id;
                }
                this._staffRepository.UpdateStaffProfile(staffProfileModel);
            }
            catch (Exception ex)
            {
                this._modelStateDictionary.AddModelError("Insert Exception For Submiting Application Form", ex.Message);
                return null;
            }

            return staffProfileModel;
        }

        public StaffProfileModel EditStaffProfile(StaffProfileModel staffProfileModel)
        {
            this._staffRepository.UpdateStaffProfile(staffProfileModel);
            return staffProfileModel;
        }

        public StaffInfoModel EditStaffInfo(StaffInfoModel staffInfoModel)
        {
            this._staffInfoContactRepository.UpdateStaffInfoContact(staffInfoModel.Contact);
            this._staffInfoContactRepository.UpdateStaffInfoContact(staffInfoModel.EmergencyContact);
            this._staffInfoRepository.UpdateStaffInfo(staffInfoModel);
            return staffInfoModel;
        }

        public StaffProfileModel RelocateStaff(StaffProfileModel staffProfileModel, DepartmentModel department)
        {
            if (department == null)
            {
                this._modelStateDictionary.AddModelError("Relocation Error", "The Department for relocating to is null.");
                return staffProfileModel;
            }
            if (staffProfileModel.DepartmentId != null && staffProfileModel.DepartmentId != 0)
            {
                var oldDepartmentId = staffProfileModel.DepartmentId;
                if (oldDepartmentId == department.Id)
                {
                    this._modelStateDictionary.AddModelError("Relocation Error", "The Department for relocation is as same as the original department.");
                    return staffProfileModel;
                }
                var oldDepartment = this._departmentRepository.Find(oldDepartmentId);
                oldDepartment.StaffProfiles.Remove(staffProfileModel);
                //= oldDepartment.StaffProfiles.Where(p => p.Id == staffProfileModel.Id).ToList();
                this._departmentRepository.UpdateDepartment(oldDepartment);
            }

            department = this._departmentRepository.GetDepartmentById(department.Id);
            if (department.StaffProfiles.Select(p => p.Id).Contains(staffProfileModel.Id))
            {
                this._modelStateDictionary.AddModelError("Relocation Error", "The Department for relocation is as same as the original department.");
                return staffProfileModel;
            }
            //notice we need to check whether the new department contains this new staff profile before assigning the relationship.
            // otherwise EF will fill in the relationship for us.
            staffProfileModel.DepartmentId = department.Id;
            this._staffRepository.UpdateStaffProfile(staffProfileModel);
            // we don't need to manually add staff into this department. becuase it has been updated....
            this._departmentRepository.UpdateDepartment(department);

            return staffProfileModel;
        }

        public StaffProfileModel ModifyPosition(StaffProfileModel staffProfileModel, PositionModel position)
        {
            if (position == null)
            {
                this._modelStateDictionary.AddModelError("Relocation Error", "The position for relocating to is null.");
                return staffProfileModel;
            }
            if (staffProfileModel.PositionId != null && staffProfileModel.PositionId != 0)
            {
                var oldpositionId = staffProfileModel.PositionId;
                if (oldpositionId == position.Id)
                {
                    this._modelStateDictionary.AddModelError("Relocation Error", "The position for relocation is as same as the original position.");
                    return staffProfileModel;
                }
            }
            staffProfileModel.PositionId = position.Id;
            position = this._positionRepository.GetPositionById(position.Id);
            staffProfileModel.Position = position;

            this._staffRepository.UpdateStaffProfile(staffProfileModel);

            return staffProfileModel;
        }

        public StaffProfileModel UpdatePayCode(StaffProfileModel staffProfileModel, ConfigPayCodeValueModel payCode)
        {
            if (payCode == null)
            {
                this._modelStateDictionary.AddModelError("Update PayCode Error", "The payCode for relocating to is null.");
                return staffProfileModel;
            }
            if (staffProfileModel.PayCodeId != null && staffProfileModel.PayCodeId != 0)
            {
                var oldpayCodeId = staffProfileModel.PayCodeId;
                if (oldpayCodeId == payCode.Id)
                {
                    this._modelStateDictionary.AddModelError("Update PayCode Error", "The payCode for relocation is as same as the original payCode.");
                    return staffProfileModel;
                }
            }
            staffProfileModel.PayCodeId = payCode.Id;
            payCode = this._payCodeRepository.GetConfigPayCodeValueById(payCode.Id);
            staffProfileModel.PayCode = payCode;

            this._staffRepository.UpdateStaffProfile(staffProfileModel);

            return staffProfileModel;
        }

        public StaffProfileModel RemoveJobDuty(StaffProfileModel staffProfileModel)
        {
            try
            {
                //if (staffProfileModel.BrandId.HasValue)
                //{
                //    var brand = this._brandRepository.Find(staffProfileModel.BrandId.Value);
                //    if (brand.Staff.Select(s => s.Id).Contains(staffProfileModel.Id))
                //    {
                //        brand.Staff.Remove(staffProfileModel);
                //        this._brandRepository.UpdateBrand(brand);
                //    }
                //}
                //if (staffProfileModel.DepartmentId.HasValue)
                //{
                //    var department = this._departmentRepository.Find(staffProfileModel.DepartmentId);
                //    if (department.StaffProfiles.Select(s => s.Id).Contains(staffProfileModel.Id))
                //    {
                //        department.StaffProfiles.Remove(staffProfileModel);
                //        this._departmentRepository.UpdateDepartment(department);
                //    }
                //}
                //if (staffProfileModel.StaffInfoId.HasValue)
                //{
                //    var staffInfo = this._staffInfoRepository.Find(staffProfileModel.StaffInfoId);
                //    if (staffInfo.StaffProfiles.Select(s => s.Id).Contains(staffProfileModel.Id))
                //    {
                //        staffInfo.StaffProfiles.Remove(staffProfileModel);
                //        this._staffInfoRepository.UpdateStaffInfo(staffInfo);
                //    }
                //}


                staffProfileModel.AdminStatus.LeaveDate = DateTimeWrapper.Now;
                staffProfileModel.AdminStatus.LeaveReason = "";
                this._staffRepository.DeleteStaffProfile(staffProfileModel.Id);
            }
            catch (Exception ex)
            {
                this._modelStateDictionary.AddModelError("Exception From Removing Job Duty", ex.Message);
                return null;
            }

            return staffProfileModel;
        }

        public StaffProfileModel DeleteStaffProfile(StaffProfileModel staffProfileModel)
        {
            this._staffRepository.DeleteStaffProfile(staffProfileModel.Id);

            this._staffProfileAdminStatusRepository.DeleteStaffProfileAdminStatus(staffProfileModel.AdminStatusId.Value);

            foreach (var leaveRecord in GetLeaveRecords(staffProfileModel))
            {
                this._staffProfileLeaveRecordReposiotry.DeleteStaffProfileLeaveRecord(leaveRecord.Id);
            }
            this._staffProfileLeaveProfileRepository.DeleteStaffProfileLeaveProfile(staffProfileModel.LeaveProfileId.Value);

            this._staffProfilePaymentDetailRepository.DeleteStaffProfilePaymentDetail(staffProfileModel.PaymentDetailId.Value);

            return staffProfileModel;
        }

        public StaffInfoModel FireStaff(StaffInfoModel staffInfoModel)
        {
            try
            {
                if (staffInfoModel.StaffProfiles != null && staffInfoModel.StaffProfiles.Any())
                {
                    // instead of calling "AsParallel" directly,
                    // here used the "ToList" to provide a clone of the original List
                    // to maintain the an unmodified list while the original list is being modified.
                    var staffProfiles = staffInfoModel.StaffProfiles.ToList();
                    staffProfiles.AsParallel().ForAll(m => RemoveJobDuty(m));
                }
                this._staffInfoRepository.DeleteStaffInfo(staffInfoModel.Id);
            }
            catch (Exception ex)
            {
                this._modelStateDictionary.AddModelError("Exception For Firing Staff", ex.Message);
                return null;
            }

            return staffInfoModel;
        }

        public StaffProfileModel ActivateStaffProfile(StaffProfileModel staffProfileModel)
        {
            staffProfileModel.Status = ObjectStatus.ACTIVE;
            staffProfileModel.LeaveProfile.Status = ObjectStatus.ACTIVE;
            staffProfileModel.LeaveProfile.LeaveRecords.AsParallel().ForAll(rec => { rec.Status = ObjectStatus.ACTIVE; });
            staffProfileModel.AdminStatus.Status = ObjectStatus.ACTIVE;
            staffProfileModel.PaymentDetail.Status = ObjectStatus.ACTIVE;
            return staffProfileModel;
        }

        public List<StaffProfileLeaveRecordModel> GetLeaveRecords(StaffProfileModel staffProfile)
        {
            var result = staffProfile.LeaveProfile.LeaveRecords.Where(rec => rec.Status != ObjectStatus.DELETED);
            return result.ToList();
        }

        public StaffProfileLeaveRecordModel InsertLeaveRecord(StaffProfileModel staffProfile, StaffProfileLeaveRecordModel leaveRecord)
        {
            leaveRecord.DateOfIssue = DateTimeWrapper.Now;
            this._staffProfileLeaveRecordReposiotry.Create(leaveRecord);
            staffProfile.LeaveProfile.LeaveRecords.Add(leaveRecord);
            this._staffProfileLeaveProfileRepository.UpdateStaffProfileLeaveProfile(staffProfile.LeaveProfile);
            return leaveRecord;
        }

        public StaffProfileLeaveRecordModel RemoveLeaveRecord(StaffProfileModel staffProfile, StaffProfileLeaveRecordModel leaveRecord)
        {
            leaveRecord.DateOfIssue = DateTimeWrapper.Now;
            this._staffProfileLeaveRecordReposiotry.DeleteStaffProfileLeaveRecord(leaveRecord.Id);
            this._staffProfileLeaveProfileRepository.UpdateStaffProfileLeaveProfile(staffProfile.LeaveProfile);
            return leaveRecord;
        }


        public StaffProfilePaymentDetailModel GetPaymentDetail(int paymentDetailId)
        {
            var paymentDetail = this._staffProfilePaymentDetailRepository.GetStaffProfilePaymentDetailById(paymentDetailId);
            return paymentDetail;
        }

        public StaffProfilePaymentDetailModel GetPaymentDetail(StaffProfileModel staffProfile)
        {
            return staffProfile.PaymentDetail;
        }

        public StaffProfilePaymentDetailModel UpdatePaymentDetail(StaffProfilePaymentDetailModel paymentDetail)
        {
            this._staffProfilePaymentDetailRepository.UpdateStaffProfilePaymentDetail(paymentDetail);
            return paymentDetail;
        }

        public StaffProfileAdminStatusModel GetAdminStatus(StaffProfileModel staffProfile)
        {
            return staffProfile.AdminStatus;
        }
        public StaffProfileAdminStatusModel GetAdminStatus(int adminStatusId)
        {
            var adminStatus = this._staffProfileAdminStatusRepository.GetStaffProfileAdminStatusById(adminStatusId);
            return adminStatus;
        }

        public StaffProfileAdminStatusModel UpdateAdminStatus(StaffProfileAdminStatusModel adminStatus)
        {
            this._staffProfileAdminStatusRepository.UpdateStaffProfileAdminStatus(adminStatus);
            return adminStatus;
        }

        public StaffProfileLeaveProfileModel InsertStaffProfileLeaveProfile(StaffProfileLeaveProfileModel StaffProfileLeaveProfileModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                var result = _staffProfileLeaveProfileRepository.Create(StaffProfileLeaveProfileModel);
                return result;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return null;
            }
        }

        public StaffProfileLeaveProfileModel GetStaffProfileLeaveProfile(StaffProfileModel staffProfile)
        {
            var dbStaffProfile = this.GetStaffProfile(staffProfile.Id);
            return dbStaffProfile.LeaveProfile;

            //var leaveProfiles = this._StaffProfileLeaveProfileRepository.GetStaffProfileLeaveProfiles();
            //var SelectedLeaveProfiles = leaveProfiles.Where(profile => profile.StaffProfileId == staffProfile.Id).FirstOrDefault();
            //return SelectedLeaveProfiles;
        }

        public StaffProfileLeaveProfileModel GetStaffProfileLeaveProfileById(int StaffProfileLeaveProfileId)
        {
            return this._staffProfileLeaveProfileRepository.GetStaffProfileLeaveProfileById(StaffProfileLeaveProfileId);
        }

        public StaffProfileLeaveProfileModel UpdateStaffProfileLeaveProfile(StaffProfileLeaveProfileModel StaffProfileLeaveProfile)
        {
            this._staffProfileLeaveProfileRepository.UpdateStaffProfileLeaveProfile(StaffProfileLeaveProfile);
            return StaffProfileLeaveProfile;
        }

        public StaffProfileLeaveProfileModel DeleteStaffProfileLeaveProfile(StaffProfileLeaveProfileModel StaffProfileLeaveProfile)
        {
            this._staffProfileLeaveProfileRepository.DeleteStaffProfileLeaveProfile(StaffProfileLeaveProfile.Id);
            return StaffProfileLeaveProfile;
        }


        public StaffInfoVisaModel GetActiveVisa(StaffInfoModel staffInfo)
        {
            var activeVisa = staffInfo.Visas
                .Where(visa => visa.Status != ObjectStatus.DELETED)
                .OrderByDescending(visa => visa.VisaExpiry);
            return activeVisa.FirstOrDefault();
        }

        public StaffInfoVisaModel UpdateVisa(StaffInfoVisaModel visa)
        {
            this._staffInfoVisaRepository.UpdateStaffInfoVisa(visa);
            return visa;
        }

        public List<StaffInfoJobHistoryModel> GetJobHistories(StaffInfoModel staffInfoModel)
        {
            var jobHistories = staffInfoModel.JobHistories.Where(hist => hist.Status != ObjectStatus.DELETED);
            return jobHistories.ToList();
        }


        public StaffInfoJobHistoryModel InsertJobHistory(StaffInfoModel staffInfo, StaffInfoJobHistoryModel jobHistory)
        {
            jobHistory.StaffInfoId = staffInfo.Id;
            this._StaffInfoJobHistoryRepository.Create(jobHistory);
            staffInfo.JobHistories.Add(jobHistory);
            this._staffInfoRepository.Update(staffInfo);
            return jobHistory;

        }
        public StaffInfoJobHistoryModel UpdateJobHistory(StaffInfoJobHistoryModel jobHistory)
        {
            this._StaffInfoJobHistoryRepository.UpdateStaffInfoJobHistory(jobHistory);
            return jobHistory;
        }
        public StaffInfoJobHistoryModel RemoveJobHistory(StaffInfoJobHistoryModel jobHistory)
        {
            this._StaffInfoJobHistoryRepository.DeleteStaffInfoJobHistory(jobHistory.Id);
            return jobHistory;
        }

        public List<StaffInfoModel> ListOnLeaveStaff(DateTime startOfLeaveDate, DateTime endOfLeaveDate)
        {
            var staffInfoes = this._staffInfoRepository.GetStaffInfoes()
                .Where(info => info.StaffProfiles.Any(profile => profile.Status != ObjectStatus.DELETED));
            var staffInfoesRequiredFocus = staffInfoes
                .Where(
                    info =>
                        info.Status != ObjectStatus.DELETED &&
                        info.StaffProfiles.Any(
                            profile =>
                                profile.Status != ObjectStatus.DELETED &&
                                profile.LeaveProfile.LeaveRecords.Any(
                                    record =>
                                        record.Status != ObjectStatus.DELETED &&
                                        record.DateReturn < endOfLeaveDate &&
                                        record.StartOfLeave <= endOfLeaveDate &&
                                        record.EndOfLeave >= startOfLeaveDate
                                )
                        )
                )
                .Include(info => info.StaffProfiles);
            return staffInfoesRequiredFocus.ToList();
        }

        public List<StaffProfileLeaveRecordModel> ListOnLeaveRecord(DateTime startOfLeaveDate, DateTime endOfLeaveDate)
        {
            var leaveRecord = this._staffProfileLeaveRecordReposiotry.GetStaffProfileLeaveRecords();
            var leaveRecordRequiredFocus = leaveRecord.Where(
                    record =>
                        record.Status != ObjectStatus.DELETED &&
                        record.DateReturn < endOfLeaveDate &&
                        record.StartOfLeave <= endOfLeaveDate &&
                        record.EndOfLeave >= startOfLeaveDate
                );
            return leaveRecordRequiredFocus.ToList();
        }



        public List<StaffInfoModel> AlertStaffVisaStatus(int months)
        {
            var alertTime = DateTimeWrapper.Now.AddMonths(months);
            var staffInfoes = this._staffInfoRepository.GetStaffInfoes().Where(info => info.StaffProfiles.Any(profile => profile.Status != ObjectStatus.DELETED));
            var staffInfoesRequiredFocus = staffInfoes
                .Where(
                    info =>
                        info.Visas.Any(
                            visa =>
                                visa.VisaExpiry <= alertTime
                        )
                )
                .Include(info => info.Visas);
            return staffInfoesRequiredFocus.ToList();
        }


        public List<StaffInfoModel> AlertStaffVevoStatus()
        {
            var staffInfoes = this._staffInfoRepository.GetStaffInfoes()
                .Where(info => info.StaffProfiles.Any(profile => profile.Status != ObjectStatus.DELETED));
            var staffInfoesRequiredFocus = staffInfoes
                .Where(
                    info =>
                        info.Visas.Any(
                            visa =>
                                visa.Status != ObjectStatus.DELETED &&
                                !visa.VevoApproved
                        )
                )
                .Include(info => info.Visas);
            return staffInfoesRequiredFocus.ToList();
        }


        public List<StaffInfoModel> AlertStaffBirthday(int days)
        {
            var alertTime = DateTimeWrapper.Now.AddDays(days);
            var staffInfoes = this._staffInfoRepository.GetStaffInfoes().Where(info => info.StaffProfiles.Any(profile => profile.Status != ObjectStatus.DELETED)).ToList();

            var staffInfoWithYearAdjustedBirthday = staffInfoes
                .Where(s => s.DateOfBirth.HasValue)
                .ToList()
                .Select(
                s => new
                {
                    StaffInfo = s,
                    DateOfBirth = (s.DateOfBirth.Value.Month == 2 && s.DateOfBirth.Value.Day == 29 ?
                        new DateTime(DateTimeWrapper.Now.Year, s.DateOfBirth.Value.Month, s.DateOfBirth.Value.Day - 1) :
                        new DateTime(DateTimeWrapper.Now.Year, s.DateOfBirth.Value.Month, s.DateOfBirth.Value.Day)
                    )
                });


            var staffInfoWithMonthAdjustedBirthday = staffInfoWithYearAdjustedBirthday
                .Select(
                    s =>
                        new
                        {
                            StaffInfo = s.StaffInfo,
                            DateOfBirth = (s.DateOfBirth <= DateTimeWrapper.Now ? s.DateOfBirth.AddYears(1) : s.DateOfBirth)
                        });
            var staffInfoesRequiredFocus = staffInfoWithMonthAdjustedBirthday
                .Where(
                    s =>
                        s.DateOfBirth <= alertTime &&
                        s.DateOfBirth >= DateTimeWrapper.Now
                )
                .Select(
                    s => s.StaffInfo
                );
            return staffInfoesRequiredFocus.ToList();
        }

        public List<StaffInfoModel> AlertStaffOnLeave()
        {
            var alertTime = DateTimeWrapper.Now;
            var trimOffTime = DateTimeWrapper.Now.AddYears(-2);

            // 2015-03-05 : use bottom-up appoarch instead (200 sth staff profile is already too many ? gosh)
            var LeaveRecords = this._staffProfileLeaveRecordReposiotry.GetStaffProfileLeaveRecords()
                .Where(record => record.StartOfLeave >= trimOffTime)
                .Where(
                    record =>
                        record.StartOfLeave <= alertTime &&
                        alertTime < record.DateReturn
                );
            var staffOnLeave = LeaveRecords.Select(record => record.StaffProfile.StaffInfo).Where(i => i.Status != ObjectStatus.DELETED).ToList();

            //// 2015-01-14 : skip reset to start of date to cater leave record that does not start at start of date.
            //var alertTime = DateTimeWrapper.StartOfDate(DateTimeWrapper.Now);
            //var staffInfoes = this._staffInfoRepository.GetStaffInfoes().Where(info => info.StaffProfiles.Any(profile => profile.Status != ObjectStatus.DELETED)).ToList();

            //var staffOnLeave = staffInfoes
            //    .Where(info =>
            //            info.StaffProfiles.Any(
            //                profile =>
            //                    profile.LeaveProfile.Status != ObjectStatus.DELETED &&
            //                    profile.LeaveProfile.LeaveRecords.Any(
            //                        record =>
            //                            record.StartOfLeave <= alertTime &&
            //                            alertTime < record.DateReturn
            //                    )
            //            )
            //    );
            return staffOnLeave.ToList();
        }

        #region GC
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
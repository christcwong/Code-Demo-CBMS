using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBMS.Models.Roster;
using CBMS.Models.Config;

namespace CBMS.Services.Interfaces.Roster
{
    public interface IStaffServices : IServices
    {
        StaffProfileModel NewApplicationForm();
        bool SubmitApplicationForm(StaffProfileModel staffProfileModel);



        /// <summary>
        /// Searching for a staff from part of its staff id or name
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        List<StaffProfileModel> SearchStaff(string queryString);
        List<StaffProfileModel> SearchStaff(string queryString, bool includeRemovedStaff);
        List<StaffProfileModel> SearchStaff(string queryString, BrandModel brand);
        List<StaffProfileModel> SearchStaff(string queryString, OutletModel outlet);
        List<StaffProfileModel> SearchStaff(string queryString, DepartmentModel department);

        StaffProfileModel GetStaffProfile(int staffProfileId);
        List<StaffProfileModel> GetStaffProfiles();

        List<StaffProfileModel> GetFiredStaff();


        StaffInfoModel GetStaffInfo(StaffProfileModel staffProfile);
        StaffInfoModel GetStaffInfo(int StaffInfoId);

        StaffProfileModel InsertStaffProfile(StaffProfileModel staffProfileModel);
        StaffProfileModel AttachStaffProfile(StaffProfileModel staffProfileModel);
        StaffProfileModel EditStaffProfile(StaffProfileModel staffProfileModel);

        StaffInfoModel EditStaffInfo(StaffInfoModel staffInfoModel);

        StaffProfileModel RelocateStaff(StaffProfileModel staffProfileModel, DepartmentModel department);

        StaffProfileModel ModifyPosition(StaffProfileModel staffProfileModel, PositionModel position);

        StaffProfileModel UpdatePayCode(StaffProfileModel staffProfileModel, ConfigPayCodeValueModel payCode);

        StaffProfileModel RemoveJobDuty(StaffProfileModel staffProfileModel);
        StaffProfileModel DeleteStaffProfile(StaffProfileModel staffProfileModel);
        StaffProfileModel ActivateStaffProfile(StaffProfileModel staffProfileModel);
        StaffInfoModel FireStaff(StaffInfoModel staffInfoModel);


        List<StaffInfoJobHistoryModel> GetJobHistories(StaffInfoModel staffInfoModel);
        StaffInfoJobHistoryModel InsertJobHistory(StaffInfoModel staffInfo, StaffInfoJobHistoryModel jobHistory);
        StaffInfoJobHistoryModel UpdateJobHistory(StaffInfoJobHistoryModel jobHistory);
        StaffInfoJobHistoryModel RemoveJobHistory(StaffInfoJobHistoryModel jobHistory);

        List<StaffProfileLeaveRecordModel> GetLeaveRecords(StaffProfileModel staffProfile);

        StaffProfileLeaveRecordModel InsertLeaveRecord(StaffProfileModel staffProfile, StaffProfileLeaveRecordModel leaveRecord);
        StaffProfileLeaveRecordModel RemoveLeaveRecord(StaffProfileModel staffProfile, StaffProfileLeaveRecordModel leaveRecord);

        StaffProfilePaymentDetailModel GetPaymentDetail(StaffProfileModel staffProfile);
        StaffProfilePaymentDetailModel GetPaymentDetail(int paymentDetailId);

        StaffProfilePaymentDetailModel UpdatePaymentDetail(StaffProfilePaymentDetailModel paymentDetail);

        StaffProfileAdminStatusModel GetAdminStatus(StaffProfileModel staffProfile);
        StaffProfileAdminStatusModel GetAdminStatus(int adminStatusId);

        StaffProfileAdminStatusModel UpdateAdminStatus(StaffProfileAdminStatusModel adminStatus);

        StaffInfoVisaModel GetActiveVisa(StaffInfoModel staffInfo);
        StaffInfoVisaModel UpdateVisa(StaffInfoVisaModel visa);

        StaffProfileLeaveProfileModel InsertStaffProfileLeaveProfile(StaffProfileLeaveProfileModel StaffProfileLeaveProfileModel);
        StaffProfileLeaveProfileModel GetStaffProfileLeaveProfile(StaffProfileModel staffProfile);
        StaffProfileLeaveProfileModel GetStaffProfileLeaveProfileById(int StaffProfileLeaveProfileId);
        StaffProfileLeaveProfileModel UpdateStaffProfileLeaveProfile(StaffProfileLeaveProfileModel StaffProfileLeaveProfile);
        StaffProfileLeaveProfileModel DeleteStaffProfileLeaveProfile(StaffProfileLeaveProfileModel StaffProfileLeaveProfile);

        List<StaffInfoModel> ListOnLeaveStaff(DateTime startOfLeaveDate, DateTime endOfLeaveDate);
        List<StaffProfileLeaveRecordModel> ListOnLeaveRecord(DateTime startOfLeaveDate, DateTime endOfLeaveDate);

        List<StaffInfoModel> AlertStaffVisaStatus(int months);
        List<StaffInfoModel> AlertStaffVevoStatus();

        List<StaffInfoModel> AlertStaffBirthday(int days);

        List<StaffInfoModel> AlertStaffOnLeave();

    }
}
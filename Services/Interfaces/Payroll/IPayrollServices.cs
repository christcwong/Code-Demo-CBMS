using CBMS.Models.Config;
using CBMS.Models.Payroll;
using CBMS.Models.Roster;
using CBMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces.Payroll
{
    public interface IPayrollServices : IServices
    {
        PayrollProfileModel InsertPayrollProfile(PayrollProfileModel payrollProfile);
        PayrollProfileModel DeletePayrollProfile(PayrollProfileModel payrollProfile);
        PayrollRecordModel DeletePayrollRecord(PayrollRecordModel payrollRecord);
        PayrollProfileModel GetPayrollProfileById(int payrollProfileId);
        List<PayrollProfileModel> GetPayrollProfiles();
        List<PayrollProfileModel> GetPayrollProfiles(OutletModel outlet);
        List<PayrollProfileModel> GetPayrollProfiles(DepartmentModel department);
        PayrollProfileModel GetLatestPayrollProfile(OutletModel outlet);
        PayrollProfileModel GetLatestPayrollProfile(DepartmentModel department);
        PayrollRecordModel UpdatePayrollRecord(PayrollRecordModel PayrollRecord);
        PayrollProfileModel UpdatePayrollProfile(PayrollProfileModel PayrollProfile);


        StaffProfileModel UpdatePayCodeOfStaff(StaffProfileModel staffProfile, ConfigPayCodeValueModel paycode);

        /// <summary>
        /// Validate the staff payroll record. return bool. check model state dictionary for errors
        /// </summary>
        /// <param name="staffPayrollRecord"></param>
        /// <returns></returns>
        bool ValidateStaffPayrollRecord(StaffPayrollRecordModel staffPayrollRecord);



        PayrollRecordModel PreparePayrollRecord(OutletModel outlet);
        PayrollRecordModel PreparePayrollRecord(OutletModel outlet, PayrollProfileModel payrollProfile);
        PayrollRecordViewModel PreparePayrollRecordViewModel(OutletModel outlet);
        PayrollRecordViewModel PreparePayrollRecordViewModel(OutletModel outlet, PayrollProfileModel payrollProfile);


        PayrollRecordModel PreparePayrollRecord(DepartmentModel department);
        PayrollRecordModel PreparePayrollRecord(DepartmentModel department, PayrollProfileModel payrollProfile);
        PayrollRecordViewModel PreparePayrollRecordViewModel(DepartmentModel department);
        PayrollRecordViewModel PreparePayrollRecordViewModel(DepartmentModel department, PayrollProfileModel payrollProfile);


        PayrollRecordModel SubmitPayrollRecord(PayrollRecordModel payrollRecord);
        List<DepartmentModel> AlertSalaryOutOfBoundDepartments();
        List<OutletModel> AlertSalaryOutOfBoundOutlets();
        List<PayrollRecordModel> AlertSalaryOutOfBoundRecords();
        List<PayrollRecordModel> AlertSalaryOutOfBoundRecords(DepartmentModel department);
        List<PayrollRecordModel> AlertSalaryOutOfBoundRecords(OutletModel outlet);
        List<PayrollProfileModel> AlertExpiringPayrollProfile(int days);
        List<PayrollProfileModel> AlertExpiringPayrollProfile(OutletModel outlet, int days);
        List<PayrollProfileModel> AlertExpiringPayrollProfile(DepartmentModel department, int days);


        List<StaffPayrollRecordModel> GetStaffPayrollRecords();
        List<StaffPayrollRecordModel> GetStaffPayrollRecords(OutletModel outlet);
        List<StaffPayrollRecordModel> GetStaffPayrollRecords(DepartmentModel department);
        StaffPayrollRecordModel GetStaffPayrollRecordById(int StaffPayrollRecordId);


        List<PayrollRecordModel> GetPayrollRecords();
        List<PayrollRecordModel> GetPayrollRecords(OutletModel outlet);
        List<PayrollRecordModel> GetPayrollRecords(DepartmentModel department);
        PayrollRecordModel GetPayrollRecordById(int PayrollRecordId);
        PayrollRecordViewModel GetPayrollRecordViewModel(PayrollRecordModel payrollRecord);

        double GetPay(ConfigPayCodeValueModel payCode, double hours, int staffProfileId);
    }
}

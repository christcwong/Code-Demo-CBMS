using CBMS.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces.Config
{
    interface IConfigurationServices : IServices
    {
        bool InsertConfigVisaType(ConfigVisaTypeModel ConfigVisaTypeModel);
        IQueryable<ConfigVisaTypeModel> GetConfigVisaTypes();
        Task<ConfigVisaTypeModel> GetConfigVisaTypeById(int ConfigVisaTypeId);
        Task UpdateConfigVisaType(ConfigVisaTypeModel ConfigVisaType);
        Task DeleteConfigVisaType(int ConfigVisaTypeId);

        bool InsertConfigPositionType(ConfigPositionTypeModel ConfigPositionTypeModel);
        IQueryable<ConfigPositionTypeModel> GetConfigPositionTypes();
        Task<ConfigPositionTypeModel> GetConfigPositionTypeById(int ConfigPositionTypeId);
        Task UpdateConfigPositionType(ConfigPositionTypeModel ConfigPositionType);
        Task DeleteConfigPositionType(int ConfigPositionTypeId);

        bool InsertConfigPayCodeValue(ConfigPayCodeValueModel ConfigPayCodeValueModel);
        IQueryable<ConfigPayCodeValueModel> GetConfigPayCodeValues();
        Task<ConfigPayCodeValueModel> GetConfigPayCodeValueById(int ConfigPayCodeValueId);
        Task UpdateConfigPayCodeValue(ConfigPayCodeValueModel ConfigPayCodeValue);
        Task DeleteConfigPayCodeValue(int ConfigPayCodeValueId);

        bool InsertConfigLeave(ConfigLeaveModel ConfigLeaveModel);
        IQueryable<ConfigLeaveModel> GetConfigLeaves();
        Task<ConfigLeaveModel> GetConfigLeaveById(int ConfigLeaveId);
        Task UpdateConfigLeave(ConfigLeaveModel ConfigLeave);
        Task DeleteConfigLeave(int ConfigLeaveId);

        bool InsertConfigBank(ConfigBankModel ConfigBankModel);
        IQueryable<ConfigBankModel> GetConfigBanks();
        Task<ConfigBankModel> GetConfigBankById(int ConfigBankId);
        Task UpdateConfigBank(ConfigBankModel ConfigBank);
        Task DeleteConfigBank(int ConfigBankId);

        bool InsertConfigShiftType(ConfigShiftTypeModel ConfigShiftTypeModel);
        IQueryable<ConfigShiftTypeModel> GetConfigShiftTypes();
        Task<ConfigShiftTypeModel> GetConfigShiftTypeById(int ConfigShiftTypeId);
        Task UpdateConfigShiftType(ConfigShiftTypeModel ConfigShiftType);
        Task DeleteConfigShiftType(int ConfigShiftTypeId);

        bool InsertConfigUnit(ConfigUnitModel ConfigUnitModel);
        IQueryable<ConfigUnitModel> GetConfigUnits();
        Task<ConfigUnitModel> GetConfigUnitById(int ConfigUnitId);
        Task UpdateConfigUnit(ConfigUnitModel ConfigUnit);
        Task DeleteConfigUnit(int ConfigUnitId);
    }
}

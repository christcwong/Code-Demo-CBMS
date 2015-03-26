using CBMS.Models.Config;
using CBMS.Services.Interfaces.Config;
using CBMS.Repositories.Interfaces.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CBMS.Models;
using System.Web.Configuration;

namespace CBMS.Services.Config
{
    public class ConfigurationServices : CBMSServices, IConfigurationServices
    {
        private string ConfigVisaTypeRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGVISATYPE"];
        private string ConfigPositionTypeRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGPOSITIONTYPE"];
        private string ConfigPayCodeValueRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGPAYCODEVALUE"];
        private string ConfigLeaveRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGLEAVE"];
        private string ConfigBankRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGBANK"];
        private string ConfigShiftTypeRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGSHIFTTYPE"];
        private string ConfigUnitRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGUNIT"];

        private IConfigShiftTypeRepository _ConfigShiftTypeRepository;
        private IConfigBankRepository _ConfigBankRepository;
        private IConfigLeaveRepository _ConfigLeaveRepository;
        private IConfigPayCodeValueRepository _ConfigPayCodeValueRepository;
        private IConfigPositionTypeRepository _ConfigPositionTypeRepository;
        private IConfigVisaTypeRepository _ConfigVisaTypeRepository;
        private IConfigUnitRepository _ConfigUnitRepository;

        public ConfigurationServices(ModelStateDictionary modelStateDictionary)
            : this(modelStateDictionary, new CBMSDbContext())
        {
        }
        public ConfigurationServices(ModelStateDictionary modelStateDictionary, CBMSDbContext dbContext)
        {
            this._modelStateDictionary = modelStateDictionary;
            this.dbContext = dbContext;
            this._ConfigVisaTypeRepository = (IConfigVisaTypeRepository)Activator.CreateInstance(Type.GetType(ConfigVisaTypeRepositoryName), dbContext);
            this._ConfigPositionTypeRepository = (IConfigPositionTypeRepository)Activator.CreateInstance(Type.GetType(ConfigPositionTypeRepositoryName), dbContext);
            this._ConfigPayCodeValueRepository = (IConfigPayCodeValueRepository)Activator.CreateInstance(Type.GetType(ConfigPayCodeValueRepositoryName), dbContext);
            this._ConfigLeaveRepository = (IConfigLeaveRepository)Activator.CreateInstance(Type.GetType(ConfigLeaveRepositoryName), dbContext);
            this._ConfigBankRepository = (IConfigBankRepository)Activator.CreateInstance(Type.GetType(ConfigBankRepositoryName), dbContext);
            this._ConfigShiftTypeRepository = (IConfigShiftTypeRepository)Activator.CreateInstance(Type.GetType(ConfigShiftTypeRepositoryName), dbContext);
            this._ConfigUnitRepository = (IConfigUnitRepository)Activator.CreateInstance(Type.GetType(ConfigUnitRepositoryName), dbContext);
        }

        #region Config Visa Type
        public bool InsertConfigVisaType(ConfigVisaTypeModel ConfigVisaTypeModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ConfigVisaTypeRepository.Create(ConfigVisaTypeModel);
                return true;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return false;
            }
        }

        public IQueryable<ConfigVisaTypeModel> GetConfigVisaTypes()
        {
            return this._ConfigVisaTypeRepository.GetConfigVisaTypes();
        }

        public async Task<ConfigVisaTypeModel> GetConfigVisaTypeById(int ConfigVisaTypeId)
        {
            return await this._ConfigVisaTypeRepository.GetConfigVisaTypeByIdAsync(ConfigVisaTypeId);
        }

        public async Task UpdateConfigVisaType(ConfigVisaTypeModel ConfigVisaType)
        {
            await this._ConfigVisaTypeRepository.UpdateConfigVisaTypeAsync(ConfigVisaType);
        }

        public async Task DeleteConfigVisaType(int ConfigVisaTypeId)
        {
            await this._ConfigVisaTypeRepository.DeleteConfigVisaTypeAsync(ConfigVisaTypeId);
        }
        #endregion

        #region Config Position Type
        public bool InsertConfigPositionType(ConfigPositionTypeModel ConfigPositionTypeModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ConfigPositionTypeRepository.Create(ConfigPositionTypeModel);
                return true;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return false;
            }
        }

        public IQueryable<ConfigPositionTypeModel> GetConfigPositionTypes()
        {
            return this._ConfigPositionTypeRepository.GetConfigPositionTypes();
        }

        public async Task<ConfigPositionTypeModel> GetConfigPositionTypeById(int ConfigPositionTypeId)
        {
            return await this._ConfigPositionTypeRepository.GetConfigPositionTypeByIdAsync(ConfigPositionTypeId);
        }

        public async Task UpdateConfigPositionType(ConfigPositionTypeModel ConfigPositionType)
        {
            await this._ConfigPositionTypeRepository.UpdateConfigPositionTypeAsync(ConfigPositionType);
        }

        public async Task DeleteConfigPositionType(int ConfigPositionTypeId)
        {
            await this._ConfigPositionTypeRepository.DeleteConfigPositionTypeAsync(ConfigPositionTypeId);
        }
        #endregion

        #region Config PayCode Value
        public bool InsertConfigPayCodeValue(ConfigPayCodeValueModel ConfigPayCodeValueModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ConfigPayCodeValueRepository.Create(ConfigPayCodeValueModel);
                return true;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return false;
            }
        }

        public IQueryable<ConfigPayCodeValueModel> GetConfigPayCodeValues()
        {
            return this._ConfigPayCodeValueRepository.GetConfigPayCodeValues();
        }

        public async Task<ConfigPayCodeValueModel> GetConfigPayCodeValueById(int ConfigPayCodeValueId)
        {
            return await this._ConfigPayCodeValueRepository.GetConfigPayCodeValueByIdAsync(ConfigPayCodeValueId);
        }

        public async Task UpdateConfigPayCodeValue(ConfigPayCodeValueModel ConfigPayCodeValue)
        {
            await this._ConfigPayCodeValueRepository.UpdateConfigPayCodeValueAsync(ConfigPayCodeValue);
        }

        public async Task DeleteConfigPayCodeValue(int ConfigPayCodeValueId)
        {
            await this._ConfigPayCodeValueRepository.DeleteConfigPayCodeValueAsync(ConfigPayCodeValueId);
        }
        #endregion

        #region Config Leave
        public bool InsertConfigLeave(ConfigLeaveModel ConfigLeaveModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ConfigLeaveRepository.Create(ConfigLeaveModel);
                return true;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return false;
            }
        }

        public IQueryable<ConfigLeaveModel> GetConfigLeaves()
        {
            return this._ConfigLeaveRepository.GetConfigLeaves();
        }

        public async Task<ConfigLeaveModel> GetConfigLeaveById(int ConfigLeaveId)
        {
            return await this._ConfigLeaveRepository.GetConfigLeaveByIdAsync(ConfigLeaveId);
        }

        public async Task UpdateConfigLeave(ConfigLeaveModel ConfigLeave)
        {
            await this._ConfigLeaveRepository.UpdateConfigLeaveAsync(ConfigLeave);
        }

        public async Task DeleteConfigLeave(int ConfigLeaveId)
        {
            await this._ConfigLeaveRepository.DeleteConfigLeaveAsync(ConfigLeaveId);
        }
        #endregion

        #region Config Bank

        public bool InsertConfigBank(ConfigBankModel ConfigBankModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ConfigBankRepository.Create(ConfigBankModel);
                return true;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return false;
            }
        }

        public IQueryable<ConfigBankModel> GetConfigBanks()
        {
            return this._ConfigBankRepository.GetConfigBanks();
        }

        public async Task<ConfigBankModel> GetConfigBankById(int ConfigBankId)
        {
            return await this._ConfigBankRepository.GetConfigBankByIdAsync(ConfigBankId);
        }

        public async Task UpdateConfigBank(ConfigBankModel ConfigBank)
        {
            await this._ConfigBankRepository.UpdateConfigBankAsync(ConfigBank);
        }

        public async Task DeleteConfigBank(int ConfigBankId)
        {
            await this._ConfigBankRepository.DeleteConfigBankAsync(ConfigBankId);
        }

        #endregion

        #region Config Shift Type
        public bool InsertConfigShiftType(ConfigShiftTypeModel ConfigShiftTypeModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ConfigShiftTypeRepository.Create(ConfigShiftTypeModel);
                return true;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return false;
            }
        }

        public IQueryable<ConfigShiftTypeModel> GetConfigShiftTypes()
        {
            return this._ConfigShiftTypeRepository.GetConfigShiftTypes();
        }

        public async Task<ConfigShiftTypeModel> GetConfigShiftTypeById(int ConfigShiftTypeId)
        {
            return await this._ConfigShiftTypeRepository.GetConfigShiftTypeByIdAsync(ConfigShiftTypeId);
        }

        public async Task UpdateConfigShiftType(ConfigShiftTypeModel ConfigShiftType)
        {
            await this._ConfigShiftTypeRepository.UpdateConfigShiftTypeAsync(ConfigShiftType);
        }

        public async Task DeleteConfigShiftType(int ConfigShiftTypeId)
        {
            await this._ConfigShiftTypeRepository.DeleteConfigShiftTypeAsync(ConfigShiftTypeId);
        }
        #endregion

        #region Config Unit

        public bool InsertConfigUnit(ConfigUnitModel ConfigUnitModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ConfigUnitRepository.Create(ConfigUnitModel);
                return true;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return false;
            }
        }

        public IQueryable<ConfigUnitModel> GetConfigUnits()
        {
            return this._ConfigUnitRepository.GetConfigUnits().OrderBy(u=>u.TypeValue).ThenBy(u=>u.Name).ThenBy(u=>u.Id);
        }

        public async Task<ConfigUnitModel> GetConfigUnitById(int ConfigUnitId)
        {
            return await this._ConfigUnitRepository.GetConfigUnitByIdAsync(ConfigUnitId);
        }

        public async Task UpdateConfigUnit(ConfigUnitModel ConfigUnit)
        {
            await this._ConfigUnitRepository.UpdateConfigUnitAsync(ConfigUnit);
        }

        public async Task DeleteConfigUnit(int ConfigUnitId)
        {
            await this._ConfigUnitRepository.DeleteConfigUnitAsync(ConfigUnitId);
        }
        #endregion
    }
}
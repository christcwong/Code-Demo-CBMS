using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBMS.Models.Roster;
using CBMS.Models.Config;


namespace CBMS.Models.ViewModels
{
    public class ConfigValueViewModel
    {
        public IList<ConfigBankModel> ConfigBanks { get; set; }
        public IList<ConfigLeaveModel> ConfigLeaves { get; set; }
        public IList<ConfigPayCodeValueModel> ConfigPayCodeValues { get; set; }
        public IList<ConfigPositionTypeModel> ConfigPositionTypes { get; set; }
        public IList<ConfigVisaTypeModel> ConfigVisaTypes { get; set; }
        public IList<ConfigShiftTypeModel> ConfigShiftTypes { get; set; }
        public IList<ConfigUnitModel> ConfigUnits { get; set; }
        public ConfigBankModel NewConfigBank { get; set; }
        public ConfigLeaveModel NewConfigLeave { get; set; }
        public ConfigPayCodeValueModel NewConfigPayCodeValue { get; set; }
        public ConfigPositionTypeModel NewConfigPositionType { get; set; }
        public ConfigVisaTypeModel NewConfigVisaType { get; set; }
        public ConfigShiftTypeModel NewConfigShiftType { get; set; }
        public ConfigUnitModel NewConfigUnit { get; set; }
    }
}
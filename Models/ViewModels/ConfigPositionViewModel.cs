using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBMS.Models.Roster;
using CBMS.Models.Config;


namespace CBMS.Models.ViewModels
{
    public class ConfigPositionViewModel
    {
        public BrandModel Brand { get; set; }
        public OutletModel Outlet { get; set; }
        public LocationModel Location { get; set; }
        public DepartmentModel Department { get; set; }
        public IList<BrandModel> Brands { get; set; }
        public IList<OutletModel> Outlets { get; set; }
        public IList<LocationModel> Locations { get; set; }
        public IList<DepartmentModel> Departments { get; set; }

        public PositionModel NewPosition { get; set; }
        public PositionModel EditPosition { get; set; }
        public IList<PositionModel> Positions { get; set; }

        public IList<ConfigPositionTypeModel> AvailableConfigPositionTypes { get; set; }

        public ConfigPayCodeValueModel ConfigPayCodeValue { get; set; }
        public IList<ConfigPayCodeValueModel> ConfigPayCodeValues { get; set; }

        public IList<ConfigPayCodeValueModel> AvailableConfigPayCodeValues { get; set; }
        public IList<ConfigPayCodeValueModel> SelectedConfigPayCodeValues { get; set; }
        public PostedConfigPayCodeValues PostedConfigPayCodeValues { get; set; }

        #region Foreign Keys
        public int DepartmentId { get; set; }

        public int PositionId { get; set; }

        public int PayCodeId { get; set; }

        public int BrandId { get; set; }

        public int OutletId { get; set; }
        #endregion

        
    }

    // Helper class to make posting back selected values easier
    public class PostedConfigPayCodeValues
    {
        public int[] IDs { get; set; }
    }
}
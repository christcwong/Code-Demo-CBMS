using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBMS.Models.Roster;


namespace CBMS.Models.ViewModels
{
    public class ConfigBrandViewModel
    {
        public BrandModel NewBrand { get; set; }
        public OutletModel NewOutlet { get; set; }
        public LocationModel NewLocation { get; set; }
        public DepartmentModel NewDepartment { get; set; }
        public IList<BrandModel> Brands { get; set; }
        public IList<OutletModel> Outlets { get; set; }
        public IList<LocationModel> Locations { get; set; }
        public IList<DepartmentModel> Departments { get; set; }
    }
}
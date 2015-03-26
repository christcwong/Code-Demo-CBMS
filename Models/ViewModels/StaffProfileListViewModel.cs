using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBMS.Models.ViewModels
{
    public class StaffProfileListViewModel
    {

        public string query { get; set; }

        public int? DepartmentId { get; set; }
        public DepartmentModel Department { get; set; }

        public int? OutletId { get; set; }
        public OutletModel Outlet { get; set; }

        public int? BrandId { get; set; }
        public BrandModel Brand { get; set; }

        public IList<StaffProfileModel> StaffProfiles { get; set; }

        public int? VisaAlertCount { get; set; }
        public int? VevoAlertCount { get; set; }
        public int? BirthdayCount { get; set; }
        public int? LeaveCount { get; set; }
    }
}
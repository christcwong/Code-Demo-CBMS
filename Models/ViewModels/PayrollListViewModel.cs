using CBMS.Models.Payroll;
using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.ViewModels
{
    /// <summary>
    /// View Model used for Viewing Payroll Of Outlet
    /// </summary>
    public class PayrollListViewModel
    {
        public OutletModel Outlet { get; set; }
        public DepartmentModel Department { get; set; }
        public IList<PayrollProfileModel> ExpiringProfiles { get; set; }
        public IList<PayrollProfileModel> PayrollProfiles { get; set; }
        public IList<PayrollRecordModel> PayrollRecords { get; set; }
        public IDictionary<PayrollRecordModel,bool> PayrollProfileExpiringDictionary { get; set; }
    }
}
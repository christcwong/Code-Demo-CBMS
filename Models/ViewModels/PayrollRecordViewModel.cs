using CBMS.Models.Payroll;
using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.ViewModels
{
    public class PayrollRecordViewModel
    {
        public PayrollRecordModel PayrollRecord { get; set; }
        public Dictionary<StaffProfileModel, List<StaffProfileLeaveRecordModel>> staffLeaves { get; set; } 

    }
}
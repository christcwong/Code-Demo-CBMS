using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.ViewModels
{
    public class StaffProfileDetailViewModel
    {
        public StaffProfileModel StaffProfile { get; set; }
        public StaffInfoVisaModel ActiveVisa { get; set; }
    }
}
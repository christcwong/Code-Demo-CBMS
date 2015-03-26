using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces.Roster
{
    public interface ILeaveServices : IServices
    {
        StaffProfileLeaveProfileModel GetStaffProfileLeaveProfile(StaffProfileModel staffProfile);
        double GetAccumulatedLeaveOfStaff(StaffProfileModel staffProfile);
        StaffProfileModel UpdateAccumulatedLeaveOfStaff(StaffProfileModel staffProfile);
    }
}

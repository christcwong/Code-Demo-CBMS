using CBMS.Models;
using CBMS.Models.Roster;
using FrameLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBMS.Services.Interfaces.Roster
{
    public interface IStaffLogServices
    {
        IEnumerable<IChangeSet<ApplicationUser>> ChangesToStaff(StaffProfileModel staffProfile);
        IEnumerable<IChangeSet<ApplicationUser>> AllChangeHistoryToStaff(StaffProfileModel staffProfile);
    }
}

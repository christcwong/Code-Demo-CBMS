﻿// <autogenerated>
//   This file was generated using IStaffProfileAdminStatusServices.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>

using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces.Roster
{
    public interface IStaffProfileAdminStatusServices : IServices
    {
        bool InsertStaffProfileAdminStatus(StaffProfileAdminStatusModel StaffProfileAdminStatusModel);
		IQueryable<StaffProfileAdminStatusModel> GetStaffProfileAdminStatus();
        Task<StaffProfileAdminStatusModel> GetStaffProfileAdminStatusById(int StaffProfileAdminStatusId);
        Task UpdateStaffProfileAdminStatus(StaffProfileAdminStatusModel StaffProfileAdminStatus);
        Task DeleteStaffProfileAdminStatus(int StaffProfileAdminStatusId);
    }
}

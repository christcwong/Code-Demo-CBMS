﻿// <autogenerated>
//   This file was generated using IStaffProfileLeaveRecordServices.tt.
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
    public interface IStaffProfileLeaveRecordServices : IServices
    {
        bool InsertStaffProfileLeaveRecord(StaffProfileLeaveRecordModel StaffProfileLeaveRecordModel);
		IQueryable<StaffProfileLeaveRecordModel> GetStaffProfileLeaveRecords();
        Task<StaffProfileLeaveRecordModel> GetStaffProfileLeaveRecordById(int StaffProfileLeaveRecordId);
        Task UpdateStaffProfileLeaveRecord(StaffProfileLeaveRecordModel StaffProfileLeaveRecord);
        Task DeleteStaffProfileLeaveRecord(int StaffProfileLeaveRecordId);
    }
}

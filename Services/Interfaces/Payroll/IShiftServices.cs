﻿// <autogenerated>
//   This file was generated using IShiftServices.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>

using CBMS.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces.Payroll
{
    public interface IShiftServices : IServices
    {
        bool InsertShift(ShiftModel ShiftModel);
		IQueryable<ShiftModel> GetShifts();
        Task<ShiftModel> GetShiftById(int ShiftId);
        Task UpdateShift(ShiftModel Shift);
        Task DeleteShift(int ShiftId);
    }
}

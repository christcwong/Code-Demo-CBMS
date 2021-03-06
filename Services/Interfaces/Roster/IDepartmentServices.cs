﻿// <autogenerated>
//   This file was generated using IDepartmentServices.tt.
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
    public interface IDepartmentServices : IServices
    {
        bool InsertDepartment(DepartmentModel DepartmentModel);
		IQueryable<DepartmentModel> GetDepartments();
        Task<DepartmentModel> GetDepartmentById(int DepartmentId);
        Task UpdateDepartment(DepartmentModel Department);
        Task DeleteDepartment(int DepartmentId);
    }
}

﻿// <autogenerated>
//   This file was generated using IPayrollProfileRepository.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>
using CBMS.Models;
using CBMS.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CBMS.Repositories.Interfaces.Payroll
{
    public interface IPayrollProfileRepository : IRepository<PayrollProfileModel>,IDisposable
    {
	    /// <summary>
        /// Insert the PayrollProfile Model into Database
        /// </summary>
        /// <param name="PayrollProfile"></param>
        #region Create
        void InsertPayrollProfile(PayrollProfileModel PayrollProfile);
        #endregion

        #region Read
        /// <summary>
        /// Get All Active PayrollProfiles
        /// </summary>
        /// <returns></returns>
        IQueryable<PayrollProfileModel> GetPayrollProfiles();
        /// <summary>
        /// Get All Active PayrollProfiles as Enumerable (Forced loading)
        /// </summary>
        /// <returns></returns>
        IEnumerable<PayrollProfileModel> GetPayrollProfilesAsList();
        /// <summary>
        /// Get PayrollProfile using its Id.
        /// </summary>
        /// <param name="PayrollProfileId"></param>
        /// <returns></returns>
        PayrollProfileModel GetPayrollProfileById(int PayrollProfileId);

        /// <summary>
        /// Get PayrollProfile using its Id. (Async)
        /// </summary>
        /// <param name="PayrollProfileId"></param>
        /// <returns></returns>
        Task<PayrollProfileModel> GetPayrollProfileByIdAsync(int PayrollProfileId);
        #endregion

        #region Update
        /// <summary>
        /// Update PayrollProfile Model.
        /// </summary>
        /// <param name="PayrollProfile"></param>
        void UpdatePayrollProfile(PayrollProfileModel PayrollProfile);
        /// <summary>
        /// Update PayrollProfile Model (Async)
        /// </summary>
        /// <param name="PayrollProfile"></param>
        /// <returns></returns>
        Task UpdatePayrollProfileAsync(PayrollProfileModel PayrollProfile);
        #endregion

        #region Delete
        /// <summary>
        /// Mark the PayrollProfile Model as Deleted in DB.
        /// </summary>
        /// <param name="PayrollProfileId"></param>
        void DeletePayrollProfile(int PayrollProfileId);
        /// <summary>
        /// Mark the PayrollProfile Model as Deleted in DB. (Async)
        /// </summary>
        /// <param name="PayrollProfileId"></param>
        /// <returns></returns>
        Task DeletePayrollProfileAsync(int PayrollProfileId);
        #endregion
    }
}
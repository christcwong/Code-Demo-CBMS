﻿// <autogenerated>
//   This file was generated using IPayrollRecordRepository.tt.
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
    public interface IPayrollRecordRepository : IRepository<PayrollRecordModel>,IDisposable
    {
	    /// <summary>
        /// Insert the PayrollRecord Model into Database
        /// </summary>
        /// <param name="PayrollRecord"></param>
        #region Create
        void InsertPayrollRecord(PayrollRecordModel PayrollRecord);
        #endregion

        #region Read
        /// <summary>
        /// Get All Active PayrollRecords
        /// </summary>
        /// <returns></returns>
        IQueryable<PayrollRecordModel> GetPayrollRecords();
        /// <summary>
        /// Get All Active PayrollRecords as Enumerable (Forced loading)
        /// </summary>
        /// <returns></returns>
        IEnumerable<PayrollRecordModel> GetPayrollRecordsAsList();
        /// <summary>
        /// Get PayrollRecord using its Id.
        /// </summary>
        /// <param name="PayrollRecordId"></param>
        /// <returns></returns>
        PayrollRecordModel GetPayrollRecordById(int PayrollRecordId);

        /// <summary>
        /// Get PayrollRecord using its Id. (Async)
        /// </summary>
        /// <param name="PayrollRecordId"></param>
        /// <returns></returns>
        Task<PayrollRecordModel> GetPayrollRecordByIdAsync(int PayrollRecordId);
        #endregion

        #region Update
        /// <summary>
        /// Update PayrollRecord Model.
        /// </summary>
        /// <param name="PayrollRecord"></param>
        void UpdatePayrollRecord(PayrollRecordModel PayrollRecord);
        /// <summary>
        /// Update PayrollRecord Model (Async)
        /// </summary>
        /// <param name="PayrollRecord"></param>
        /// <returns></returns>
        Task UpdatePayrollRecordAsync(PayrollRecordModel PayrollRecord);
        #endregion

        #region Delete
        /// <summary>
        /// Mark the PayrollRecord Model as Deleted in DB.
        /// </summary>
        /// <param name="PayrollRecordId"></param>
        void DeletePayrollRecord(int PayrollRecordId);
        /// <summary>
        /// Mark the PayrollRecord Model as Deleted in DB. (Async)
        /// </summary>
        /// <param name="PayrollRecordId"></param>
        /// <returns></returns>
        Task DeletePayrollRecordAsync(int PayrollRecordId);
        #endregion
    }
}
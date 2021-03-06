﻿// <autogenerated>
//   This file was generated using IStaffInfoRepository.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>
using CBMS.Models;
using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CBMS.Repositories.Interfaces.Roster
{
    public interface IStaffInfoRepository : IRepository<StaffInfoModel>,IDisposable
    {
	    /// <summary>
        /// Insert the StaffInfo Model into Database
        /// </summary>
        /// <param name="StaffInfo"></param>
        #region Create
        void InsertStaffInfo(StaffInfoModel StaffInfo);
        #endregion

        #region Read
        /// <summary>
        /// Get All Active StaffInfos
        /// </summary>
        /// <returns></returns>
        IQueryable<StaffInfoModel> GetStaffInfoes();
        /// <summary>
        /// Get All Active StaffInfoes as Enumerable (Forced loading)
        /// </summary>
        /// <returns></returns>
        IEnumerable<StaffInfoModel> GetStaffInfoesAsList();
        /// <summary>
        /// Get StaffInfo using its Id.
        /// </summary>
        /// <param name="StaffInfoId"></param>
        /// <returns></returns>
        StaffInfoModel GetStaffInfoById(int StaffInfoId);

        /// <summary>
        /// Get StaffInfo using its Id. (Async)
        /// </summary>
        /// <param name="StaffInfoId"></param>
        /// <returns></returns>
        Task<StaffInfoModel> GetStaffInfoByIdAsync(int StaffInfoId);
        #endregion

        #region Update
        /// <summary>
        /// Update StaffInfo Model.
        /// </summary>
        /// <param name="StaffInfo"></param>
        void UpdateStaffInfo(StaffInfoModel StaffInfo);
        /// <summary>
        /// Update StaffInfo Model (Async)
        /// </summary>
        /// <param name="StaffInfo"></param>
        /// <returns></returns>
        Task UpdateStaffInfoAsync(StaffInfoModel StaffInfo);
        #endregion

        #region Delete
        /// <summary>
        /// Mark the StaffInfo Model as Deleted in DB.
        /// </summary>
        /// <param name="StaffInfoId"></param>
        void DeleteStaffInfo(int StaffInfoId);
        /// <summary>
        /// Mark the StaffInfo Model as Deleted in DB. (Async)
        /// </summary>
        /// <param name="StaffInfoId"></param>
        /// <returns></returns>
        Task DeleteStaffInfoAsync(int StaffInfoId);
        #endregion
    }
}
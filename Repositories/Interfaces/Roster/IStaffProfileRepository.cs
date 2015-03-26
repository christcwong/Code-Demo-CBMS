﻿// <autogenerated>
//   This file was generated using IStaffProfileRepository.tt.
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
    public interface IStaffProfileRepository : IRepository<StaffProfileModel>,IDisposable
    {
	    /// <summary>
        /// Insert the StaffProfile Model into Database
        /// </summary>
        /// <param name="StaffProfile"></param>
        #region Create
        void InsertStaffProfile(StaffProfileModel StaffProfile);
        #endregion

        #region Read
        /// <summary>
        /// Get All Active StaffProfiles
        /// </summary>
        /// <returns></returns>
        IQueryable<StaffProfileModel> GetStaffProfiles();
        /// <summary>
        /// Get All Active StaffProfiles as Enumerable (Forced loading)
        /// </summary>
        /// <returns></returns>
        IEnumerable<StaffProfileModel> GetStaffProfilesAsList();
        /// <summary>
        /// Get StaffProfile using its Id.
        /// </summary>
        /// <param name="StaffProfileId"></param>
        /// <returns></returns>
        StaffProfileModel GetStaffProfileById(int StaffProfileId);

        /// <summary>
        /// Get StaffProfile using its Id. (Async)
        /// </summary>
        /// <param name="StaffProfileId"></param>
        /// <returns></returns>
        Task<StaffProfileModel> GetStaffProfileByIdAsync(int StaffProfileId);
        #endregion

        #region Update
        /// <summary>
        /// Update StaffProfile Model.
        /// </summary>
        /// <param name="StaffProfile"></param>
        void UpdateStaffProfile(StaffProfileModel StaffProfile);
        /// <summary>
        /// Update StaffProfile Model (Async)
        /// </summary>
        /// <param name="StaffProfile"></param>
        /// <returns></returns>
        Task UpdateStaffProfileAsync(StaffProfileModel StaffProfile);
        #endregion

        #region Delete
        /// <summary>
        /// Mark the StaffProfile Model as Deleted in DB.
        /// </summary>
        /// <param name="StaffProfileId"></param>
        void DeleteStaffProfile(int StaffProfileId);
        /// <summary>
        /// Mark the StaffProfile Model as Deleted in DB. (Async)
        /// </summary>
        /// <param name="StaffProfileId"></param>
        /// <returns></returns>
        Task DeleteStaffProfileAsync(int StaffProfileId);
        #endregion
    }
}
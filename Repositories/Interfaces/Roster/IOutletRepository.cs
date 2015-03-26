﻿// <autogenerated>
//   This file was generated using IOutletRepository.tt.
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
    public interface IOutletRepository : IRepository<OutletModel>,IDisposable
    {
	    /// <summary>
        /// Insert the Outlet Model into Database
        /// </summary>
        /// <param name="Outlet"></param>
        #region Create
        void InsertOutlet(OutletModel Outlet);
        #endregion

        #region Read
        /// <summary>
        /// Get All Active Outlets
        /// </summary>
        /// <returns></returns>
        IQueryable<OutletModel> GetOutlets();
        /// <summary>
        /// Get All Active Outlets as Enumerable (Forced loading)
        /// </summary>
        /// <returns></returns>
        IEnumerable<OutletModel> GetOutletsAsList();
        /// <summary>
        /// Get Outlet using its Id.
        /// </summary>
        /// <param name="OutletId"></param>
        /// <returns></returns>
        OutletModel GetOutletById(int OutletId);

        /// <summary>
        /// Get Outlet using its Id. (Async)
        /// </summary>
        /// <param name="OutletId"></param>
        /// <returns></returns>
        Task<OutletModel> GetOutletByIdAsync(int OutletId);
        #endregion

        #region Update
        /// <summary>
        /// Update Outlet Model.
        /// </summary>
        /// <param name="Outlet"></param>
        void UpdateOutlet(OutletModel Outlet);
        /// <summary>
        /// Update Outlet Model (Async)
        /// </summary>
        /// <param name="Outlet"></param>
        /// <returns></returns>
        Task UpdateOutletAsync(OutletModel Outlet);
        #endregion

        #region Delete
        /// <summary>
        /// Mark the Outlet Model as Deleted in DB.
        /// </summary>
        /// <param name="OutletId"></param>
        void DeleteOutlet(int OutletId);
        /// <summary>
        /// Mark the Outlet Model as Deleted in DB. (Async)
        /// </summary>
        /// <param name="OutletId"></param>
        /// <returns></returns>
        Task DeleteOutletAsync(int OutletId);
        #endregion
    }
}
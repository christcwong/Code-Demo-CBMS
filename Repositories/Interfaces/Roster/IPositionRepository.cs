﻿// <autogenerated>
//   This file was generated using IPositionRepository.tt.
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
    public interface IPositionRepository : IRepository<PositionModel>,IDisposable
    {
	    /// <summary>
        /// Insert the Position Model into Database
        /// </summary>
        /// <param name="Position"></param>
        #region Create
        void InsertPosition(PositionModel Position);
        #endregion

        #region Read
        /// <summary>
        /// Get All Active Positions
        /// </summary>
        /// <returns></returns>
        IQueryable<PositionModel> GetPositions();
        /// <summary>
        /// Get All Active Positions as Enumerable (Forced loading)
        /// </summary>
        /// <returns></returns>
        IEnumerable<PositionModel> GetPositionsAsList();
        /// <summary>
        /// Get Position using its Id.
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        PositionModel GetPositionById(int PositionId);

        /// <summary>
        /// Get Position using its Id. (Async)
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        Task<PositionModel> GetPositionByIdAsync(int PositionId);
        #endregion

        #region Update
        /// <summary>
        /// Update Position Model.
        /// </summary>
        /// <param name="Position"></param>
        void UpdatePosition(PositionModel Position);
        /// <summary>
        /// Update Position Model (Async)
        /// </summary>
        /// <param name="Position"></param>
        /// <returns></returns>
        Task UpdatePositionAsync(PositionModel Position);
        #endregion

        #region Delete
        /// <summary>
        /// Mark the Position Model as Deleted in DB.
        /// </summary>
        /// <param name="PositionId"></param>
        void DeletePosition(int PositionId);
        /// <summary>
        /// Mark the Position Model as Deleted in DB. (Async)
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        Task DeletePositionAsync(int PositionId);
        #endregion
    }
}
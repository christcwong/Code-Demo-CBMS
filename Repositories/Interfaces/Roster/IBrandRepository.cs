﻿// <autogenerated>
//   This file was generated using IBrandRepository.tt.
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
    public interface IBrandRepository : IRepository<BrandModel>,IDisposable
    {
	    /// <summary>
        /// Insert the Brand Model into Database
        /// </summary>
        /// <param name="Brand"></param>
        #region Create
        void InsertBrand(BrandModel Brand);
        #endregion

        #region Read
        /// <summary>
        /// Get All Active Brands
        /// </summary>
        /// <returns></returns>
        IQueryable<BrandModel> GetBrands();
        /// <summary>
        /// Get All Active Brands as Enumerable (Forced loading)
        /// </summary>
        /// <returns></returns>
        IEnumerable<BrandModel> GetBrandsAsList();
        /// <summary>
        /// Get Brand using its Id.
        /// </summary>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        BrandModel GetBrandById(int BrandId);

        /// <summary>
        /// Get Brand using its Id. (Async)
        /// </summary>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        Task<BrandModel> GetBrandByIdAsync(int BrandId);
        #endregion

        #region Update
        /// <summary>
        /// Update Brand Model.
        /// </summary>
        /// <param name="Brand"></param>
        void UpdateBrand(BrandModel Brand);
        /// <summary>
        /// Update Brand Model (Async)
        /// </summary>
        /// <param name="Brand"></param>
        /// <returns></returns>
        Task UpdateBrandAsync(BrandModel Brand);
        #endregion

        #region Delete
        /// <summary>
        /// Mark the Brand Model as Deleted in DB.
        /// </summary>
        /// <param name="BrandId"></param>
        void DeleteBrand(int BrandId);
        /// <summary>
        /// Mark the Brand Model as Deleted in DB. (Async)
        /// </summary>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        Task DeleteBrandAsync(int BrandId);
        #endregion
    }
}
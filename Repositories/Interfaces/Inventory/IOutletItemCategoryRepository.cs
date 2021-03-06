﻿// <autogenerated>
//   This file was generated using IOutletItemsRepository.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>
using CBMS.Models;
using CBMS.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CBMS.Repositories.Interfaces.Inventory
{
    public interface IOutletItemCategoryRepository : IRepository<OutletItemCategoryModel>, IDisposable
    {
        /// <summary>
        /// Insert the OutletItems Model into Database
        /// </summary>
        /// <param name="OutletItems"></param>
        #region Create
        void InsertOutletItemCategory(OutletItemCategoryModel OutletItemCategory);
        #endregion

        #region Read
        /// <summary>
        /// Get All Active OutletItemss
        /// </summary>
        /// <returns></returns>
        IQueryable<OutletItemCategoryModel> GetOutletItemCategories();
        /// <summary>
        /// Get All Active OutletItemss as Enumerable (Forced loading)
        /// </summary>
        /// <returns></returns>
        IEnumerable<OutletItemCategoryModel> GetOutletItemCategoriesAsList();
        /// <summary>
        /// Get OutletItems using its Id.
        /// </summary>
        /// <param name="OutletItemsId"></param>
        /// <returns></returns>
        OutletItemCategoryModel GetOutletItemCategoryById(int OutletItemCategoryId);

        /// <summary>
        /// Get OutletItems using its Id. (Async)
        /// </summary>
        /// <param name="OutletItemsId"></param>
        /// <returns></returns>
        Task<OutletItemCategoryModel> GetOutletItemCategoryByIdAsync(int OutletItemCategoryId);
        #endregion

        #region Update
        /// <summary>
        /// Update OutletItems Model.
        /// </summary>
        /// <param name="OutletItems"></param>
        void UpdateOutletItemCategory(OutletItemCategoryModel OutletItemCategory);
        /// <summary>
        /// Update OutletItems Model (Async)
        /// </summary>
        /// <param name="OutletItems"></param>
        /// <returns></returns>
        Task UpdateOutletItemCategoryAsync(OutletItemCategoryModel OutletItemCategory);
        #endregion

        #region Delete
        /// <summary>
        /// Mark the OutletItems Model as Deleted in DB.
        /// </summary>
        /// <param name="OutletItemsId"></param>
        void DeleteOutletItemCategory(int OutletItemCategoryId);
        /// <summary>
        /// Mark the OutletItems Model as Deleted in DB. (Async)
        /// </summary>
        /// <param name="OutletItemsId"></param>
        /// <returns></returns>
        Task DeleteOutletItemCategoryAsync(int OutletItemCategoryId);
        #endregion
    }
}
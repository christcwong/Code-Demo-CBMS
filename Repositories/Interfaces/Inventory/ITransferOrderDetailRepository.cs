﻿// <autogenerated>
//   This file was generated using ITransferOrderDetailRepository.tt.
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
    public interface ITransferOrderDetailRepository : IRepository<TransferOrderDetailModel>,IDisposable
    {
	    /// <summary>
        /// Insert the TransferOrderDetail Model into Database
        /// </summary>
        /// <param name="TransferOrderDetail"></param>
        #region Create
        void InsertTransferOrderDetail(TransferOrderDetailModel TransferOrderDetail);
        #endregion

        #region Read
        /// <summary>
        /// Get All Active TransferOrderDetails
        /// </summary>
        /// <returns></returns>
        IQueryable<TransferOrderDetailModel> GetTransferOrderDetails();
        /// <summary>
        /// Get All Active TransferOrderDetails as Enumerable (Forced loading)
        /// </summary>
        /// <returns></returns>
        IEnumerable<TransferOrderDetailModel> GetTransferOrderDetailsAsList();
        /// <summary>
        /// Get TransferOrderDetail using its Id.
        /// </summary>
        /// <param name="TransferOrderDetailId"></param>
        /// <returns></returns>
        TransferOrderDetailModel GetTransferOrderDetailById(int TransferOrderDetailId);

        /// <summary>
        /// Get TransferOrderDetail using its Id. (Async)
        /// </summary>
        /// <param name="TransferOrderDetailId"></param>
        /// <returns></returns>
        Task<TransferOrderDetailModel> GetTransferOrderDetailByIdAsync(int TransferOrderDetailId);
        #endregion

        #region Update
        /// <summary>
        /// Update TransferOrderDetail Model.
        /// </summary>
        /// <param name="TransferOrderDetail"></param>
        void UpdateTransferOrderDetail(TransferOrderDetailModel TransferOrderDetail);
        /// <summary>
        /// Update TransferOrderDetail Model (Async)
        /// </summary>
        /// <param name="TransferOrderDetail"></param>
        /// <returns></returns>
        Task UpdateTransferOrderDetailAsync(TransferOrderDetailModel TransferOrderDetail);
        #endregion

        #region Delete
        /// <summary>
        /// Mark the TransferOrderDetail Model as Deleted in DB.
        /// </summary>
        /// <param name="TransferOrderDetailId"></param>
        void DeleteTransferOrderDetail(int TransferOrderDetailId);
        /// <summary>
        /// Mark the TransferOrderDetail Model as Deleted in DB. (Async)
        /// </summary>
        /// <param name="TransferOrderDetailId"></param>
        /// <returns></returns>
        Task DeleteTransferOrderDetailAsync(int TransferOrderDetailId);
        #endregion
    }
}
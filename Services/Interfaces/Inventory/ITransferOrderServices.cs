﻿// <autogenerated>
//   This file was generated using ITransferOrderServices.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>

using CBMS.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces.Inventory
{
    public interface ITransferOrderServices : IServices
    {
        bool InsertTransferOrder(TransferOrderModel TransferOrderModel);
		IQueryable<TransferOrderModel> GetTransferOrders();
        Task<TransferOrderModel> GetTransferOrderById(int TransferOrderId);
        Task UpdateTransferOrder(TransferOrderModel TransferOrder);
        Task DeleteTransferOrder(int TransferOrderId);
    }
}

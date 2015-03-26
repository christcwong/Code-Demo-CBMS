﻿// <autogenerated>
//   This file was generated using IItemServices.tt.
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
    public interface IItemServices : IServices
    {
        bool InsertItem(ItemModel ItemModel);
		IQueryable<ItemModel> GetItems();
        Task<ItemModel> GetItemById(int ItemId);
        Task UpdateItem(ItemModel Item);
        Task DeleteItem(int ItemId);
    }
}

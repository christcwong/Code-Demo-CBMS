﻿// <autogenerated>
//   This file was generated using IItemCategoryServices.tt.
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
    public interface IItemCategoryServices : IServices
    {
        bool InsertItemCategory(ItemCategoryModel ItemCategoryModel);
		IQueryable<ItemCategoryModel> GetItemCategories();
        Task<ItemCategoryModel> GetItemCategoryById(int ItemCategoryId);
        Task UpdateItemCategory(ItemCategoryModel ItemCategory);
        Task DeleteItemCategory(int ItemCategoryId);
    }
}

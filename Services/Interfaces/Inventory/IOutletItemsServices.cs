﻿// <autogenerated>
//   This file was generated using IOutletItemsServices.tt.
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
    public interface IOutletItemsServices : IServices
    {
        bool InsertOutletItemCategory(OutletItemCategoryModel OutletItemsModel);
        IQueryable<OutletItemCategoryModel> GetOutletItemCategories();
        Task<OutletItemCategoryModel> GetOutletItemCategoryById(int OutletItemCategoryId);
        Task UpdateOutletItemCategory(OutletItemCategoryModel OutletItemCategory);
        Task DeleteOutletItemCategory(int OutletItemCategoryId);
    }
}
﻿// <autogenerated>
//   This file was generated using IBrandServices.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>

using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces.Roster
{
    public interface IBrandServices : IServices
    {
        bool InsertBrand(BrandModel BrandModel);
        IQueryable<BrandModel> GetBrands();
        BrandModel GetBrandById(int BrandId);
        Task<BrandModel> GetBrandByIdAsync(int BrandId);
        IEnumerable<OutletModel> GetOutlets(BrandModel brand);
        Task UpdateBrand(BrandModel Brand);
        Task DeleteBrand(int BrandId);
    }
}
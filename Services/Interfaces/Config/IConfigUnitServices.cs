﻿// <autogenerated>
//   This file was generated using IConfigUnitServices.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>

using CBMS.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces.Config
{
    public interface IConfigUnitServices : IServices
    {
        bool InsertConfigUnit(ConfigUnitModel ConfigUnitModel);
		IQueryable<ConfigUnitModel> GetConfigUnits();
        Task<ConfigUnitModel> GetConfigUnitById(int ConfigUnitId);
        Task UpdateConfigUnit(ConfigUnitModel ConfigUnit);
        Task DeleteConfigUnit(int ConfigUnitId);
    }
}

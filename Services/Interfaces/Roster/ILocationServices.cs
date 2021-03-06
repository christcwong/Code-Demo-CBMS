﻿// <autogenerated>
//   This file was generated using ILocationServices.tt.
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
    public interface ILocationServices : IServices
    {
        bool InsertLocation(LocationModel LocationModel);
		IQueryable<LocationModel> GetLocations();
        Task<LocationModel> GetLocationById(int LocationId);
        Task UpdateLocation(LocationModel Location);
        Task DeleteLocation(int LocationId);
    }
}

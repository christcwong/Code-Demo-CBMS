﻿// <autogenerated>
//   This file was generated using ShiftServices.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>

using CBMS.Models.Payroll;
using CBMS.Services.Interfaces.Payroll;
using CBMS.Repositories.Interfaces.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CBMS.Models;
using System.Web.Configuration;

namespace CBMS.Services.Payroll
{
    public class ShiftServices : CBMSServices,IShiftServices
    {
		private string ShiftRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.PAYROLL.SHIFT"];
	
        private IShiftRepository _ShiftRepository;
		
        public ShiftServices(ModelStateDictionary modelStateDictionary)
        {
            this._modelStateDictionary = modelStateDictionary;
			this.dbContext = new CBMSDbContext();
            this._ShiftRepository = (IShiftRepository)Activator.CreateInstance(Type.GetType(ShiftRepositoryName), dbContext);
        }
		public ShiftServices(ModelStateDictionary modelStateDictionary,CBMSDbContext dbContext)
        {
            this._modelStateDictionary = modelStateDictionary;
			this.dbContext = dbContext;
            this._ShiftRepository = (IShiftRepository)Activator.CreateInstance(Type.GetType(ShiftRepositoryName), dbContext);
        }
		
        public bool InsertShift(ShiftModel ShiftModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ShiftRepository.Create(ShiftModel);
                return true;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return false;
            }
        }

		public IQueryable<ShiftModel> GetShifts(){
			return this._ShiftRepository.GetShifts();
		}
		
        public async Task<ShiftModel> GetShiftById(int ShiftId)
        {
            return await this._ShiftRepository.GetShiftByIdAsync(ShiftId);
        }

        public async Task UpdateShift(ShiftModel Shift)
        {
            await this._ShiftRepository.UpdateShiftAsync(Shift);
        }

        public async Task DeleteShift(int ShiftId)
        {
            await this._ShiftRepository.DeleteShiftAsync(ShiftId);
        }
		
    }
}
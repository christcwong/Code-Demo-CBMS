using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CBMS.Utilities;
using FrameLog;
using CBMS.Models.FrameLog;
using FrameLog.Contexts;
using FrameLog.History;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using FrameLog.Filter;
using System.Data.Entity.Validation;

namespace CBMS.Models
{
    public class CBMSDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public readonly FrameLogModule<ChangeSet, ApplicationUser> Logger;

        public CBMSDbContext()
            : base("name=DefaultConnection")
        {
            //Database.Log = s => Console.WriteLine(s); 
            var logContext = new CBMSDbContextAdapter(this);
            Logger = new FrameLogModule<ChangeSet, ApplicationUser>(new ChangeSetFactory(), logContext);
        }
        #region configure tables
        public DbSet<Config.ConfigBankModel> ConfigBanks { get; set; }
        public DbSet<Config.ConfigLeaveModel> ConfigLeaves { get; set; }
        public DbSet<Config.ConfigPayCodeValueModel> ConfigPayCodeValues { get; set; }
        public DbSet<Config.ConfigPositionTypeModel> ConfigPositionTypes { get; set; }
        public DbSet<Config.ConfigVisaTypeModel> ConfigVisaTypes { get; set; }
        public DbSet<Config.ConfigShiftTypeModel> ConfigShiftTypes { get; set; }
        public DbSet<Config.ConfigUnitModel> ConfigUnits { get; set; }
        #endregion

        #region roster tables
        public DbSet<Roster.StaffInfoModel> StaffInfoes { get; set; }
        public DbSet<Roster.StaffProfileModel> StaffProfiles { get; set; }
        public DbSet<Roster.BrandModel> Brands { get; set; }
        public DbSet<Roster.DepartmentModel> Departments { get; set; }
        public DbSet<Roster.LocationModel> Locations { get; set; }
        public DbSet<Roster.OutletModel> Outlets { get; set; }
        public DbSet<Roster.PositionModel> Positions { get; set; }
        public DbSet<Roster.StaffInfoContactModel> StaffInfoContacts { get; set; }
        public DbSet<Roster.StaffInfoJobHistoryModel> StaffInfoJobHistories { get; set; }
        public DbSet<Roster.StaffInfoVisaModel> StaffInfoVisas { get; set; }
        public DbSet<Roster.StaffProfileAdminStatusModel> StaffProfileAdminStatus { get; set; }
        public DbSet<Roster.StaffProfileLeaveProfileModel> StaffProfileLeaveProfileModels { get; set; }
        public DbSet<Roster.StaffProfileLeaveRecordModel> StaffProfileLeaveRecords { get; set; }
        public DbSet<Roster.StaffProfilePaymentDetailModel> StaffProfilePaymentDetails { get; set; }
        #endregion

        #region payroll tables
        public DbSet<Payroll.PayrollProfileModel> PayrollProfiles { get; set; }
        public DbSet<Payroll.PayrollRecordModel> PayrollRecords { get; set; }
        public DbSet<Payroll.StaffPayrollRecordModel> StaffPayrollRecords { get; set; }
        public DbSet<Payroll.ShiftModel> Shifts { get; set; }
        #endregion

        #region inventory tables
        public DbSet<Inventory.ItemCategoryModel> ItemCateogries { get; set; }
        public DbSet<Inventory.ItemModel> Items { get; set; }
        public DbSet<Inventory.OutletItemCategoryModel> OutletItemCategories { get; set; }
        public DbSet<Inventory.TransferOrderModel> TransferOrders { get; set; }
        public DbSet<Inventory.TransferOrderDetailModel> TransferOrderDetails { get; set; }
        #endregion

        #region FrameLog
        public DbSet<FrameLog.ChangeSet> ChangeSets { get; set; }
        public DbSet<FrameLog.ObjectChange> ObjectChanges { get; set; }
        public DbSet<FrameLog.PropertyChange> PropertyChanges { get; set; }


        /// <summary>
        ///  Currently it just use the save changes in dbcontext.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw new DbEntityValidationException(e.GetEntityValidationErrors(),e);
            }
        }

        public int SaveChanges(ApplicationUser author)
        {

            //return this.SaveChanges();
            return Logger.SaveChangesWithinExplicitTransaction(author);
        }

        public async Task<int> SaveChangesAsync(ApplicationUser author)
        {
            //return await this.SaveChangesAsync();
            return await Task<int>.Run(() => Logger.SaveChangesWithinExplicitTransaction(author));
        }

        public IFrameLogContext<ChangeSet, ApplicationUser> FrameLogContext
        {
            get { return new CBMSDbContextAdapter(this); }
        }
        public CBMSHistoryExplorer<ChangeSet, ApplicationUser> HistoryExplorer
        {
            get { return new CBMSHistoryExplorer<ChangeSet, ApplicationUser>(FrameLogContext); }
        }

        #endregion


        #region Customized Role
        public DbSet<Accessiblity> Accessibilities { get; set; }
        #endregion

        #region ApplicationDbContext
        public static CBMSDbContext Create()
        {
            return new CBMSDbContext();
        }
        #endregion
    }
}
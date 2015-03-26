using CBMS.Models;
using CBMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CBMS.Services
{
    public class CBMSServices : IServices,IDisposable
    {
        protected ModelStateDictionary _modelStateDictionary;
        protected DbContextTransaction _transaction;
        protected CBMSDbContext dbContext;

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                this._transaction = this.dbContext.Database.BeginTransaction();
            }
        }
        public void BeginTransaction(System.Data.IsolationLevel level)
        {
            if (_transaction == null)
            {
                this._transaction = this.dbContext.Database.BeginTransaction(level);
            }
        }


        public void Commit()
        {
            if (_transaction != null)
            {
                this._transaction.Commit();
                this._transaction.Dispose();
                this._transaction = null;
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                this._transaction.Rollback();
                this._transaction.Dispose();
                this._transaction = null;
            }
        }

        public int SaveChanges()
        {
            return this.dbContext.SaveChanges();
        }
        public int SaveChanges(ApplicationUser author)
        {
            return this.dbContext.SaveChanges(author);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.dbContext.SaveChangesAsync();
        }
        public async Task<int> SaveChangesAsync(ApplicationUser author)
        {
            return await this.dbContext.SaveChangesAsync(author);
        }

        #region GC
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
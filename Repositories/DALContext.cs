using CBMS.Models;
using CBMS.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CBMS.Repositories
{
    public class CBMSDALContext :IDALContext
    {
        private CBMSDbContext dbContext;
        private DbContextTransaction transaction;

        public CBMSDALContext()
        {
            dbContext = new CBMSDbContext();
        }

        public void Begin()
        {
            if (this.transaction == null)
            {
                this.transaction = this.dbContext.Database.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (this.transaction != null)
            {
                this.transaction.Commit();
                this.transaction.Dispose();
                this.transaction = null;
            }
        }

        public void Rollback()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
                this.transaction.Dispose();
                this.transaction = null;
            }
        }

        //public ICategoryRepository Categories
        //{
        //    get
        //    {
        //        if (categories == null)
        //            categories = new CategoryRepository(dbContext);
        //        return categories;
        //    }
        //}

        //public IProductRepository Products
        //{
        //    get
        //    {
        //        if (products == null)
        //            products = new ProductRepostiroy(dbContext);
        //        return products;
        //    }
        //}

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (dbContext != null)
                dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
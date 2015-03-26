using CBMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces
{
    public interface IServices : IDisposable
    {
        void BeginTransaction();
        void BeginTransaction(System.Data.IsolationLevel level);
        void Commit();
        void Rollback();
        int SaveChanges();
        int SaveChanges(ApplicationUser author);
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(ApplicationUser author);
    }
}

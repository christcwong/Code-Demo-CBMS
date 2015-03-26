using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services
{
    public interface IDALContext : IUnitOfWork
    {
        void Begin();
        void Commit();
        void Rollback();
    }
}

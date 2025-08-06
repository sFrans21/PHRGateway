using APIWHD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Data
{
    public interface ITransactionDashboardService
    {
        Task<List<TransactionDashboardItem>> GetTransactionDashboardAsync(string userName);
        Task<List<GetUserRoleMobile>> GetUserRole(string userName);
    }
    public class TransactionDashboardService : ITransactionDashboardService
    {
        private readonly APIDBContext _context;
        public TransactionDashboardService(APIDBContext context)
        {
            _context = context;
        }

        public async Task<List<TransactionDashboardItem>> GetTransactionDashboardAsync(string userName)
        {
            var result = await _context.TransactionDashboardItems.FromSqlRaw("exec USP_getTransDashboard {0}", userName).ToListAsync();
            
            return result;
        }
        public async Task<List<GetUserRoleMobile>> GetUserRole(string username)
        {
            var result = await _context.GetUserRoleMobile.FromSqlRaw("EXEC USP_GetUserRoleMobile {0}", username).ToListAsync();
            return result;
        }
    }

}

using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Infrastructure;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories
{
    public interface IConfigRepository : IBaseRepository<Config>
    {
    }
    public class ConfigRepository : BaseRepository<Config>, IConfigRepository
    {
        private readonly QuanLyVatLieuXayDungDbContext _context;
        public ConfigRepository(QuanLyVatLieuXayDungDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

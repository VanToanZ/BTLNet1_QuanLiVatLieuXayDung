using BTL_QuanLyVatLieuXayDung.Data.Infrastructure;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Common;

namespace BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories
{
    public interface INhapRepostiory : IBaseRepository<Nhap>
    {
       
    }
    public class NhapRepostiory : BaseRepository<Nhap>, INhapRepostiory
    {
        private readonly QuanLyVatLieuXayDungDbContext _context;
        public NhapRepostiory(QuanLyVatLieuXayDungDbContext context) : base(context)
        {
            _context = context;
        }

       
    }
}

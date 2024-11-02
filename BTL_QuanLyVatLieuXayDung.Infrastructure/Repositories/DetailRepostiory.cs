using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Infrastructure;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Common;

namespace BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories
{
    public interface IDetailHoaDonRepostiory : IBaseRepository<DetailHoaDon>
    {
    }
    public class DetailHoaDonRepostiory : BaseRepository<DetailHoaDon>, IDetailHoaDonRepostiory
    {
        private readonly QuanLyVatLieuXayDungDbContext _context;
        public DetailHoaDonRepostiory(QuanLyVatLieuXayDungDbContext context) : base(context)
        {
            _context = context;
        }


    }
}

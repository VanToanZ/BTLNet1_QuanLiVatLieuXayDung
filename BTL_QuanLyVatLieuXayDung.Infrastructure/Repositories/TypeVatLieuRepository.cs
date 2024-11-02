using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Infrastructure;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories
{
    public interface ITypeVatLieuRepository : IBaseRepository<TypeVatLieu>
    {
        Task<bool> ExistCodeTypeVatLieu(string code);
        Task<bool> ExistTypeVatLieuByCodeAndDiffrentId(string code, string id);
    }
    public class TypeVatLieuRepository : BaseRepository<TypeVatLieu>, ITypeVatLieuRepository
    {
        private readonly QuanLyVatLieuXayDungDbContext _context;
        public TypeVatLieuRepository(QuanLyVatLieuXayDungDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistCodeTypeVatLieu(string code)
        {
            return await _context.TypeVatLieu.AnyAsync(x => x.CodeTypeVatLieu.Equals(code)
                                                          && x.Status != nameof(EStatus.Delete));

        }

        public async Task<bool> ExistTypeVatLieuByCodeAndDiffrentId(string code, string id)
        {
            return await _context.TypeVatLieu
                .AnyAsync(x => x.CodeTypeVatLieu.Equals(code)
                               && x.Id != id
                               && x.Status != nameof(EStatus.Delete));
        }
    }
}

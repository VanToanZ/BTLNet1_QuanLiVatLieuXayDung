using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Infrastructure;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories
{
    public interface IVatLieuRepository : IBaseRepository<VatLieu>
    {
        Task<bool> ExistCodeVatLieu(string code);
        Task<bool> ExistVatLieuByTypeVatLieuId(string Id);
        Task<bool> ExistVatLieuByTypeContainerId(string Id);
        Task<bool> ExistTypeVatLieuByCodeAndDiffrentId(string code, string id);
    }
    public class VatLieuRepository : BaseRepository<VatLieu>, IVatLieuRepository
    {
        private readonly QuanLyVatLieuXayDungDbContext _context;
        public VatLieuRepository(QuanLyVatLieuXayDungDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistCodeVatLieu(string code)
        {
            return await _context.VatLieu
                .AnyAsync(x => x.CodeVatLieu.Equals(code)
                              && x.Status != nameof(EStatus.Delete));

        }

        public async Task<bool> ExistTypeVatLieuByCodeAndDiffrentId(string code, string id)
        {
            return await _context.TypeVatLieu
                .AnyAsync(x => x.CodeTypeVatLieu.Equals(code)
                               && x.Id != id
                               && x.Status != nameof(EStatus.Delete));
        }

        public async Task<bool> ExistVatLieuByTypeVatLieuId(string Id)
        {
            return await _context.VatLieu
                .AnyAsync(x => x.TypeVatLieuId.Equals(Id));
        }
        public async Task<bool> ExistVatLieuByTypeContainerId(string Id)
        {
            return await _context.VatLieu
                .AnyAsync(x => x.ContainerId.Equals(Id));
        }
    }
}

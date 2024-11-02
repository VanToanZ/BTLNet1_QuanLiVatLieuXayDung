using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Infrastructure;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories
{
    public interface IContainerRepository : IBaseRepository<Container>
    {
        Task<bool> ExistCodeContainer(string code);
        Task<bool> ExistContainerByCodeAndDiffrentId(string code, string id);
    }
    public class ContainerRepository : BaseRepository<Container>, IContainerRepository
    {
        private readonly QuanLyVatLieuXayDungDbContext _context;
        public ContainerRepository(QuanLyVatLieuXayDungDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistCodeContainer(string code)
        {
            return await _context.Container
                .AnyAsync(x => x.CodeContainer.Equals(code)
                               && x.Status != nameof(EStatus.Delete));

        }

        public async Task<bool> ExistContainerByCodeAndDiffrentId(string code, string id)
        {
            return await _context.Container
                .AnyAsync(x => x.CodeContainer.Equals(code)
                               && x.Id != id
                               && x.Status != nameof(EStatus.Delete));
        }
    }
}

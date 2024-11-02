using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Infrastructure;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> ExistUserName(string username);
        Task<User?> GetAccount(string username, string password);
        Task<User?> GetAccountByUserName(string username);
        Task<bool> ExistAccountByUserNameAndDiffrentId(string username, string id);
    }
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        private readonly QuanLyVatLieuXayDungDbContext _context;
        public UserRepository(QuanLyVatLieuXayDungDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistUserName(string username)
        {
            return await _context.User.AnyAsync(x => x.UserName.Equals(username));
            
        }
        public async Task<User?> GetAccount(string username, string password)
        {
            return await _context.User
                .FirstOrDefaultAsync(x => x.UserName.Equals(username)
                               && x.Password.Equals(password) );

        }

        public async Task<User?> GetAccountByUserName(string username)
        {
            return await _context.User
                .FirstOrDefaultAsync(x => x.UserName.Equals(username)
                                          && x.Status != nameof(EStatus.Delete));
        }

        public async Task<bool> ExistAccountByUserNameAndDiffrentId(string username, string id)
        {
            return await _context.User
                .AnyAsync(x => x.UserName.Equals(username)
                               && x.Id != id
                               && x.Status != nameof(EStatus.Delete));
        }
    }
}

using BTL_QuanLyVatLieuXayDung.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_QuanLyVatLieuXayDung.Data.Common.Seeds
{
    public class UserSeeds
    {
        public static IEnumerable<User> GetUsers()
        {
            var admin = new User()
            {
                UserName = "admin",
                FullName = "Admin",
                Password = "123",
                Address = "Adress",
                CCCD = "1234567890",
                Email = "Address@email.com",
                Role = nameof(ETypeUser.Admin),
                Status = nameof(EStatus.Active),

            };
            return new List<User>
            {
                admin
            };
        }
    }
}

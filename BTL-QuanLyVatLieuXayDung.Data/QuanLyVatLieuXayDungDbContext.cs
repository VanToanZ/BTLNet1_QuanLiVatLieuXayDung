using BTL_QuanLyVatLieuXayDung.Data.Common.Seeds;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BTL_QuanLyVatLieuXayDung.Data
{
    public class QuanLyVatLieuXayDungDbContext : DbContext
    {
        public QuanLyVatLieuXayDungDbContext(DbContextOptions<QuanLyVatLieuXayDungDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(UserSeeds.GetUsers()) ;

        }

        public DbSet<User> User { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Container> Container { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<DetailHoaDon> DetailHoaDon { get; set; }
        public DbSet<Nhap> Nhap { get; set; }
        public DbSet<TypeVatLieu> TypeVatLieu { get; set; }
        public DbSet<VatLieu> VatLieu { get; set; }
    }
}

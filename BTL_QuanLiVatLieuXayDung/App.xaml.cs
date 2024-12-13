using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Windows;

namespace BTL_QuanLiVatLieuXayDung
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            const string connectionString = "Data Source=MSI;Initial Catalog=QuanLyVatLieuXayDung;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True"; AppHost = Host.CreateDefaultBuilder()
                 .ConfigureLogging(logging =>
                 {
                     logging.ClearProviders(); // Clear default logging providers
                     logging.AddConsole(); // Add console logging
                                           // Optionally, you can add file logging or any other provider
                 })
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddDbContext<QuanLyVatLieuXayDungDbContext>(options =>
                     {
                         options.UseSqlServer(connectionString, builder =>
                             builder.MigrationsAssembly(typeof(QuanLyVatLieuXayDungDbContext).Assembly.FullName));
                     });
                     //// Đăng ký DbContext
                     //services.AddScoped<QuanLyVatLieuXayDungDbContext>(provider =>
                     //    new QuanLyVatLieuXayDungDbContext());

                     // Đăng ký các dịch vụ khác
                     services.AddScoped<IUserRepository, UserRepository>();
                     services.AddScoped<ITypeVatLieuRepository, TypeVatLieuRepository>();
                     services.AddScoped<IVatLieuRepository, VatLieuRepository>();
                     services.AddScoped<IContainerRepository, ContainerRepository>();
                     services.AddScoped<INhapRepostiory, NhapRepostiory>();
                     services.AddScoped<IHoaDonRepository, HoaDonRepository>();
                     services.AddScoped<IDetailHoaDonRepostiory, DetailHoaDonRepostiory>();
                     services.AddScoped<IConfigRepository, ConfigRepository>();
                     services.AddTransient<MainWindow>();
                 }).Build();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var startUpForm = AppHost!.Services.GetRequiredService<MainWindow>();
            startUpForm.Show();
        }       
    }
}

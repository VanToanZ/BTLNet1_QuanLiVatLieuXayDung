using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace BTL_QuanLiVatLieuXayDung
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        private ServiceProvider serviceProvider;
        public App()
        {
            const string connectionString = "Data Source=MSI;Initial Catalog=QuanLyVatLieuXayDung_V4;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
            AppHost = Host.CreateDefaultBuilder()
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
                     services.AddTransient<MainWindow>();
                 }).Build();
        }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    var mainWindow = serviceProvider.GetService<MainWindow>();
        //    mainWindow!.Show();
        //    //await AppHost!.StartAsync();
        //    //var startUpForm = AppHost.Services.GetRequiredService<MainWindow>();
        //    //startUpForm.Show();
        //    //base.OnStartup(e);
        //}
        //protected override async void OnExit(ExitEventArgs e)
        //{
        //    await AppHost!.StopAsync();
        //    base.OnExit(e);
        //}

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var startUpForm = AppHost!.Services.GetRequiredService<MainWindow>();
            startUpForm.Show();
        }
        //const string connectionString = "Data Source=MSI;Initial Catalog=QuanLyVatLieuXayDung_V4;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
        //private ServiceProvider serviceProvider;
        //public App()
        //{
        //    ServiceCollection services = new ServiceCollection();
        //    ConfigureServices(services);
        //    serviceProvider = services.BuildServiceProvider();
        //}
        //private void ConfigureServices(ServiceCollection services)
        //{
        //    services.AddDbContext<QuanLyVatLieuXayDungDbContext>(options =>
        //                {
        //                    options.UseSqlServer(connectionString, builder =>
        //                        builder.MigrationsAssembly(typeof(QuanLyVatLieuXayDungDbContext).Assembly.FullName));
        //                });
        //    services.AddScoped<IUserRepository, UserRepository>();
        //    services.AddScoped<ITypeVatLieuRepository, TypeVatLieuRepository>();
        //    services.AddScoped<IVatLieuRepository, VatLieuRepository>();
        //    services.AddScoped<IContainerRepository, ContainerRepository>();
        //    services.AddScoped<INhapRepostiory, NhapRepostiory>();
        //    services.AddScoped<IHoaDonRepository, HoaDonRepository>();
        //    services.AddScoped<IDetailHoaDonRepostiory, DetailHoaDonRepostiory>();
        //    services.AddSingleton<MainWindow>();
        //}
        //private void OnStartup(object sender, StartupEventArgs e)
        //{
        //        var userAdd = new User()
        //        {
        //            UserName = "admin",
        //            FullName = "Admin",
        //            Password = "123",
        //            Address = "Adress",
        //            CCCD = "1234567890",
        //            Email = "Address@email.com",
        //            Role = nameof(ETypeUser.Admin),
        //            Status = nameof(EStatus.Active),

        //        };
        //        context.User.Add(userAdd);
        //        context.SaveChanges();
        //    }
        //    var mainWindow = serviceProvider.GetService<MainWindow>();
        //    mainWindow.Show();
        //}
    }
}

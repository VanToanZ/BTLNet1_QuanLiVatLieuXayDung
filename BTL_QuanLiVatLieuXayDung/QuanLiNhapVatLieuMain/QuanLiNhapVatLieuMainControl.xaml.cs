using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace BTL_QuanLiVatLieuXayDung.QuanLiNhapVatLieuMain
{
    /// <summary>
    /// Interaction logic for QuanLiNhapVatLieuMainControl.xaml
    /// </summary>
    public partial class QuanLiNhapVatLieuMainControl : UserControl
    {
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly INhapRepostiory _nhapRepostiory;
        public QuanLiNhapVatLieuMainControl(
            IVatLieuRepository vatLieuRepository,
            INhapRepostiory nhapRepostiory)
        {
            InitializeComponent();
            _vatLieuRepository = vatLieuRepository;
            _nhapRepostiory = nhapRepostiory;
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            var nhapVatLieus = _nhapRepostiory
                .FindByCondition(x => !x.Status.Equals(nameof(EStatus.Delete)));
            var selectedVatLieu = searchVatLieu.SelectedItem as KeyValueItem;
            if (selectedVatLieu != null)
            {
                nhapVatLieus = nhapVatLieus.Where(x => x.VatLieuId == selectedVatLieu.Key);
            }

            nhapVatLieus = nhapVatLieus.Include(x => x.VatLieuForeignKey);
            var nhapVatLieuExcute = await nhapVatLieus.ToListAsync();
            var nhapVatLieuDtos = nhapVatLieuExcute.Select(x => new NhapDto()
            {
                Id = x.Id,
                NameVatLieu = x.VatLieuForeignKey.NameVatLieu,
                Quantity = x.Quantity,
                Price = x.Price,
                TotalMoney = x.TotalMoney,
                Status = x.Status
            }).ToList();
            dataNhap.ItemsSource = nhapVatLieuDtos;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiNhapVatLieuMainControl(_vatLieuRepository, _nhapRepostiory));
        }
        private async void NhapVatLieu_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            searchVatLieu.ItemsSource = await GetVatLieus();
            dataNhap.ItemsSource = await GetNhapVatLieus();

        }
        private async Task<List<KeyValueItem>> GetVatLieus()
        {
            var containers = await _vatLieuRepository
               .FindByCondition(x => x.Status != nameof(EStatus.Delete))
               .ToListAsync();
            var keyValueItems = containers.Select(x => new KeyValueItem()
            {
                Key = x.Id,
                Value = x.NameVatLieu
            }).ToList();
            return keyValueItems;
        }
        private async Task<List<NhapDto>> GetNhapVatLieus()
        {
            var nhapVatLieus = await _nhapRepostiory
               .FindByCondition(x => x.Status != nameof(EStatus.Delete))
               .Include(x => x.VatLieuForeignKey)
               .ToListAsync();
            var nhapVatLieuDtos = nhapVatLieus.Select(x => new NhapDto()
            {
                Id = x.Id,
                NameVatLieu = x.VatLieuForeignKey.NameVatLieu,
                Quantity = x.Quantity,
                Price = x.Price,
                TotalMoney = x.TotalMoney,
                Status = x.Status
            });
            return nhapVatLieuDtos.ToList();
        }
        private void LoadUserControl(UserControl userControl)
        {
            Content = userControl;
        }
    }
}

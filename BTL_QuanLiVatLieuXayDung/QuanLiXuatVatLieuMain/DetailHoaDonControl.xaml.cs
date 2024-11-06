using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace BTL_QuanLiVatLieuXayDung.QuanLiXuatVatLieuMain
{
    /// <summary>
    /// Interaction logic for DetailHoaDonControl.xaml
    /// </summary>
    public partial class DetailHoaDonControl : UserControl
    {
        private string _id;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        public DetailHoaDonControl(
            string id, 
            IVatLieuRepository vatLieuRepository,
            IDetailHoaDonRepostiory detailHoaDonRepostiory)
        {
            InitializeComponent();
            _id = id;
            _vatLieuRepository = vatLieuRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
        }

        private async void search_Click(object sender, RoutedEventArgs e)
        {
            var sql = _detailHoaDonRepostiory
              .FindByCondition(x => !x.Status.Equals(nameof(EStatus.Delete))
                                    && x.HoaDonId == _id);
            var selectedVatLieu = searchVatLieu.SelectedItem as KeyValueItem;
            if (selectedVatLieu != null)
            {
                sql = sql.Where(x => x.VatLieuId.Equals(selectedVatLieu.Key));
            }
            var sqlExute = await sql.Include(x =>x.VatLieuForeignKey).ToListAsync();
            var detailHoaDonDtos = sqlExute.Select(x => new DeTailHoaDonDto()
            {
                NameVatLieu = x.VatLieuForeignKey.NameVatLieu,
                Quanity = x.Quanity,
                TotalMoney = x.TotalMoney.ToString(),
            }).ToList();
            
            dataDetailHoaDon.ItemsSource = detailHoaDonDtos;
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new DetailHoaDonControl(_id, _vatLieuRepository, _detailHoaDonRepostiory));
        }
        private async void DetailHoaDon_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }
        private async Task LoadData()
        {
            searchVatLieu.ItemsSource = await GetVatLieus();
            dataDetailHoaDon.ItemsSource = await GetDetailHoaDons();

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
        private async Task<List<DeTailHoaDonDto>> GetDetailHoaDons()
        {
            var detailHoaDons = await _detailHoaDonRepostiory
               .FindByCondition(x => x.Status != nameof(EStatus.Delete)
                                     && x.HoaDonId == _id)
               .Include(x => x.VatLieuForeignKey)
               .ToListAsync();
            var detailHoaDonDtos = detailHoaDons.Select(x => new DeTailHoaDonDto()
            {
                
                NameVatLieu = x.VatLieuForeignKey.NameVatLieu,
                Quanity = x.Quanity,
                TotalMoney = x.TotalMoney.ToString(),
            });
            return detailHoaDonDtos.ToList();
        }
        private void LoadUserControl(UserControl userControl)
        {

            Content = userControl;
        }
    }
}

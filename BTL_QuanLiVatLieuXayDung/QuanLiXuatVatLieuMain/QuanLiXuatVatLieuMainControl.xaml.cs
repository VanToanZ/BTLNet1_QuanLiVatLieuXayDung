using BTL_QuanLiVatLieuXayDung.QuanLiVatLieuMain;
using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BTL_QuanLiVatLieuXayDung.QuanLiXuatVatLieuMain
{
    /// <summary>
    /// Interaction logic for QuanLiXuatVatLieuMainControl.xaml
    /// </summary>
    public partial class QuanLiXuatVatLieuMainControl : UserControl
    {
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        public QuanLiXuatVatLieuMainControl(
            IHoaDonRepository hoaDonRepository,
             IVatLieuRepository vatLieuRepository,
             IDetailHoaDonRepostiory detailHoaDonRepostiory
            )
        {
            InitializeComponent();
            _hoaDonRepository = hoaDonRepository;
            _vatLieuRepository = vatLieuRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
        }

        private async void search_Click(object sender, RoutedEventArgs e)
        {
            var sql = _hoaDonRepository
               .FindByCondition(x => !x.Status.Equals(nameof(EStatus.Delete)));
            var codeHoaDon = searchCodeHoaDon.Text;
            if (codeHoaDon != null)
            {
                sql = sql.Where(x => x.CodeHoaDon.Equals(codeHoaDon));
            }

            var hoaDonDtos = await sql.Select(x => new HoaDonDto()
            {
                Id = x.Id,
                CodeHoaDon = x.CodeHoaDon,
                AddressNhan = x.AddressNhan,
                DescriptionHoaDon = x.DescriptionHoaDon,
                SoDienThoai = x.SoDienThoai,
                TotalMoney = x.TotalMoney,
            }).ToListAsync();
            dataHoaDon.ItemsSource = hoaDonDtos;
        }

        private async void HoaDon_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }
        private async Task LoadData()
        {
            dataHoaDon.ItemsSource = await GetNhapVatLieus();

        }
        private async Task<List<HoaDonDto>> GetNhapVatLieus()
        {
            var hoaDons = await _hoaDonRepository
               .FindByCondition(x => x.Status != nameof(EStatus.Delete))
               .ToListAsync();
            var hoaDonDtos = hoaDons.Select(x => new HoaDonDto()
            {
                Id = x.Id,
                CodeHoaDon = x.CodeHoaDon,
                AddressNhan = x.AddressNhan,
                DescriptionHoaDon = x.DescriptionHoaDon,
                SoDienThoai = x.SoDienThoai,
                TotalMoney = x.TotalMoney,
            });
            return hoaDonDtos.ToList();
        }
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiXuatVatLieuMainControl(
                _hoaDonRepository
                ,_vatLieuRepository,
                _detailHoaDonRepostiory));
        }
        private void LoadUserControl(UserControl userControl)
        {
            Content = userControl;
        }
        private async void Deatail_Click(object sender, RoutedEventArgs e)
        {
            if (dataHoaDon.SelectedItem is HoaDonDto selectedUser)
            {
                var hoaDon = await _hoaDonRepository.GetByIdAsync(selectedUser.Id);
                if (hoaDon != null)
                {
                    LoadUserControl(new DetailHoaDonControl(
                        hoaDon.Id,              
                       _vatLieuRepository,
                       _detailHoaDonRepostiory));
                }
            }
        }
    }
}

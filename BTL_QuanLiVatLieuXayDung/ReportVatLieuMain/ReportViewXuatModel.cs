using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using LiveCharts;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BTL_QuanLiVatLieuXayDung.ReportVatLieuMain
{
    public class ReportViewXuatModel : INotifyPropertyChanged
    {
        private ObservableCollection<XuatVatLieuReportDto> _vatLieuAndXuatData;
        public ObservableCollection<XuatVatLieuReportDto> VatLieuAndXuatData
        {
            get { return _vatLieuAndXuatData; }
            set
            {
                _vatLieuAndXuatData = value;
                OnPropertyChanged();
            }
        }

        public ChartValues<double> TotalXuat { get; set; }
        public ChartValues<string> Labels { get; set; } // To hold the labels (VatLieu names)
        public ChartValues<double> TotalPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand FilterCommand { get; set; }

        public ReportViewXuatModel(
            IHoaDonRepository hoaDonRepository,
            IDetailHoaDonRepostiory detailHoaDonRepostiory,
            DateTime? _startDate,
            DateTime? _endDate)
        {
            _hoaDonRepository = hoaDonRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
            FilterCommand = new Relay1Command(FilterData); // Implement a RelayCommand to trigger filtering
            // Set default values for date filters (e.g., last 1 month)
            StartDate = _startDate is null ? DateTime.Now.AddMonths(-1) : _startDate;
            EndDate = _endDate is null ? DateTime.Now : _endDate;
            LoadData(); // Initial data load
        }

        private async void LoadData()
        {
            // If no date range is specified, use the default values
            var startDate = StartDate ?? DateTime.Now.AddMonths(-1);
            var endDate = EndDate ?? DateTime.Now;

            // Retrieve the filtered data from the repository
            var hoaDons = await _hoaDonRepository
                .FindByCondition(n => n.CreateAt.Date >= startDate
                                      && n.CreateAt.Date <= endDate)
                .ToListAsync();
            var hoaDonIds = hoaDons.Select(x => x.Id);
            var deatu = await _detailHoaDonRepostiory
                .FindByCondition(x => true).ToListAsync();
            var deatailHoaDons = await _detailHoaDonRepostiory
                .FindByCondition(x => hoaDonIds.Contains(x.HoaDonId))
                .Include(x => x.VatLieuForeignKey)
                .Include(x => x.HoaDonForeignKey)
                .ToListAsync();
            // Group data by Date and VatLieu.NameVatLieu, and then calculate total quantity and price
            var groupedData = deatailHoaDons
                .GroupBy(n => new { n.VatLieuForeignKey.NameVatLieu, n.VatLieuForeignKey.Id, n.VatLieuForeignKey.UrlImage })
                .Select(g => new XuatVatLieuReportDto()
                {
                    NameVatLieu = g.Key.NameVatLieu,
                    Picture = File.ReadAllBytes(g.Key.UrlImage!),
                    TotalQuantity = g.Sum(n => n.Quanity),
                    TotalMoney = g.Sum(n => n.Quanity * n.VatLieuForeignKey.Price)
                })
                .ToList();
            // Set the data for the DataGrid
            VatLieuAndXuatData = new ObservableCollection<XuatVatLieuReportDto>(groupedData);
            // Set up data for the chart
            TotalXuat = new ChartValues<double>(groupedData.Select(g => g.TotalQuantity));
            TotalPrice = new ChartValues<double>(groupedData.Select(g => g.TotalMoney));
            // Create labels from the grouped data to display material names in the chart
            Labels = new ChartValues<string>(groupedData.Select(g => g.NameVatLieu));
            OnPropertyChanged(nameof(TotalXuat));
            OnPropertyChanged(nameof(TotalPrice));
            OnPropertyChanged(nameof(Labels));
        }

        private void FilterData()
        {
            LoadData(); // Reload data based on the filtered date range
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // RelayCommand implementation for ICommand (you can add this in a separate file or in the same file)
    public class Relay1Command : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public Relay1Command(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();
    }
}

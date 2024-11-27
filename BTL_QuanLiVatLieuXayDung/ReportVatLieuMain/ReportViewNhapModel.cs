using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using LiveCharts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BTL_QuanLiVatLieuXayDung.ReportVatLieuMain
{
    public class ReportViewNhapModel : INotifyPropertyChanged
    {
        private ObservableCollection<NhapReportDto> _vatLieuAndNhapData;
        public ObservableCollection<NhapReportDto> VatLieuAndNhapData
        {
            get { return _vatLieuAndNhapData; }
            set
            {
                _vatLieuAndNhapData = value;
                OnPropertyChanged();
            }
        }

        public ChartValues<double> TotalNhap { get; set; }
        public ChartValues<double> TotalPrice { get; set; }
        public ChartValues<string> Labels { get; set; } // To hold the labels (VatLieu names)

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        private readonly INhapRepostiory _nhapRepostiory;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand FilterCommand { get; set; }

        public ReportViewNhapModel(
            INhapRepostiory nhapRepostiory,
            DateTime? _startDate,
            DateTime? _endDate)
        {
            _nhapRepostiory = nhapRepostiory;
            FilterCommand = new RelayCommand(FilterData); // Implement a RelayCommand to trigger filtering
            // Set default values for date filters (e.g., last 1 month)
            StartDate = _startDate is null ? DateTime.Now.AddMonths(-1) : _startDate;
            EndDate = _endDate is null ? DateTime.Now : _endDate;
            LoadData(); // Initial data load
        }

        private void LoadData()
        {
            // If no date range is specified, use the default values
            var startDate = StartDate ?? DateTime.Now.AddMonths(-1);
            var endDate = EndDate ?? DateTime.Now;

            // Retrieve the filtered data from the repository
            var data = _nhapRepostiory.FindByCondition(n => n.CreateAt.Date >= startDate 
                                                            && n.CreateAt.Date <= endDate)
                                       .Include(n => n.VatLieuForeignKey)
                                       .ToList();

            // Group data by Date and VatLieu.NameVatLieu, and then calculate total quantity and price
            var groupedData = data
                .GroupBy(n => new { n.CreateAt.Date, n.VatLieuForeignKey.NameVatLieu, n.VatLieuForeignKey.UrlImage })
                .Select(g => new NhapReportDto()
                {
                    NgayNhap = g.Key.Date,
                    NameVatLieu = g.Key.NameVatLieu,
                    Picture = File.ReadAllBytes(g.Key.UrlImage!),
                    TotalQuantity = g.Sum(n => n.Quantity),
                    TotalPrice = g.Sum(n => n.Quantity * n.Price) // Total price for each group
                })
                .ToList();
            // Set the data for the DataGrid
            VatLieuAndNhapData = new ObservableCollection<NhapReportDto>(groupedData);
            // Set up data for the chart
            TotalNhap = new ChartValues<double>(groupedData.Select(g => g.TotalQuantity));
            TotalPrice = new ChartValues<double>(groupedData.Select(g => g.TotalPrice));

            // Create labels from the grouped data to display material names in the chart
            Labels = new ChartValues<string>(groupedData.Select(g => g.NameVatLieu +  "-" + g.NgayNhap.ToString("yyyy-MM-dd")).ToList());

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
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
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

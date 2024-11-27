using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using OfficeOpenXml;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BTL_QuanLiVatLieuXayDung.ReportVatLieuMain
{
    /// <summary>
    /// Interaction logic for ReportXuatVatLieuMainControl.xaml
    /// </summary>
    public partial class ReportXuatVatLieuMainControl : UserControl
    {
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        private readonly IHoaDonRepository _hoaDonRepository;
        public ReportXuatVatLieuMainControl(
            IDetailHoaDonRepostiory detailHoaDonRepostiory,
            IHoaDonRepository hoaDonRepository)
        {
            InitializeComponent();
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
            _hoaDonRepository = hoaDonRepository;
            this.DataContext = new ReportViewXuatModel(_hoaDonRepository, _detailHoaDonRepostiory, null, null);
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var startDateTime = Convert.ToDateTime(StartDatePicker.Text);
            var endDateTime = Convert.ToDateTime(EndDatePicker.Text);
            this.DataContext = new ReportViewXuatModel(_hoaDonRepository, _detailHoaDonRepostiory, startDateTime, endDateTime);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ReportViewXuatModel(_hoaDonRepository, _detailHoaDonRepostiory, null, null);
        }
        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            // Get the list of reports from the DataContext
            var reportData = (this.DataContext as ReportViewXuatModel)?.VatLieuAndXuatData;

            if (reportData == null || reportData.Count == 0)
            {
                MessageBox.Show("No data available for export.", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                var username = Application.Current.Properties["username"]?.ToString() ?? "";
                // Add a worksheet to the Excel file
                var worksheet = package.Workbook.Worksheets.Add("Nhap Report");
                worksheet.Cells[1, 1].Value = $"Người xuất báo cáo: {username}";
                worksheet.Cells[2, 1].Value = $"Từ: {StartDatePicker.Text:yyyy-MM-dd}";
                worksheet.Cells[2, 2].Value = $"Đến: {EndDatePicker.Text:yyyy-MM-dd}";
                worksheet.Cells[3, 1].Value = $"Từ:  {DateTime.Now.ToString():yyyy-MM-dd}";
                worksheet.Cells[4, 1].Value = $"Báo cáo: Xuất hàng";
                // Set headers
                worksheet.Cells[5, 1].Value = "STT";
                worksheet.Cells[5, 2].Value = "Tên vật liệu";
                worksheet.Cells[5, 3].Value = "Ảnh loại vật liệu"; // Optional: You can handle image export or skip it
                worksheet.Cells[5, 4].Value = "Số lượng nhập";
                worksheet.Cells[5, 5].Value = "Tổng tiền";

                // Set data rows
                int row = 6; // Start from row 2 because row 1 is for headers
                int count = 0;
                foreach (var item in reportData)
                {
                    worksheet.Cells[row, 1].Value = ++ count;
                    worksheet.Cells[row, 2].Value = item.NameVatLieu;
                    worksheet.Cells[row, 3].Value = "Image";
                    worksheet.Cells[row, 4].Value = item.TotalQuantity;
                    worksheet.Cells[row, 5].Value = item.TotalMoney;
                    row++;
                }

                // Save the Excel file to the disk
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    FileName = "Xuất_Report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var filePath = saveFileDialog.FileName;
                    var file = new FileInfo(filePath);
                    package.SaveAs(file);
                    MessageBox.Show("Exported successfully!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}

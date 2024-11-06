using BTL_QuanLiVatLieuXayDung.QuanLiTaiKhoanMain;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace BTL_QuanLiVatLieuXayDung.QuanLiKhuVucHangMain
{
    /// <summary>
    /// Interaction logic for QuanLiKhuVucHangControl.xaml
    /// </summary>
    public partial class QuanLiKhuVucHangControl : UserControl
    {
        private readonly IContainerRepository _containerRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        public QuanLiKhuVucHangControl(IContainerRepository containerRepository, IVatLieuRepository vatLieuRepository)
        {
            InitializeComponent();
            _containerRepository = containerRepository;
            _vatLieuRepository = vatLieuRepository;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new CreateKhuVucHangControl(_containerRepository, _vatLieuRepository));
        }
        private async void quanLiKhuVuc_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }
        private async Task LoadData()
        {
            var data = await GetData();
            dataContainer.ItemsSource = data;
        }

        private async Task<List<ContainerDto>> GetData()
        {
            var containers = await _containerRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
                .ToListAsync();

            return containers.Select(x => new ContainerDto
            {
                Id = x.Id,
                NameContainer = x.NameContainer,
                CodeContainer = x.CodeContainer,
                Picture = File.ReadAllBytes(x.UrlImage),
                DescriptionContainer = x.DescriptionContainer,
                Status = x.Status,
                IsChecked = x.Status.Equals(nameof(EStatus.Active)) ? true : false,
            }).ToList();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiKhuVucHangControl(_containerRepository, _vatLieuRepository));
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            var sql = _containerRepository
                .FindByCondition(x => !x.Status.Equals(nameof(EStatus.Delete)));
            string name = searchNameContainer.Text;

            if (!string.IsNullOrEmpty(name))
            {
                sql = sql.Where(x => x.NameContainer.Contains(name));
            }
            string code = searchMaContainer.Text;
            if (!string.IsNullOrEmpty(code))
            {
                sql = sql.Where(x => x.CodeContainer.Contains(code));
            }
            var containers = await sql.ToListAsync();
            var containerDtos = containers.Select(x => new ContainerDto()
            {
                Id = x.Id,
                NameContainer = x.NameContainer,
                CodeContainer = x.CodeContainer,
                DescriptionContainer = x.DescriptionContainer,
                Picture = File.ReadAllBytes(x.UrlImage),
                Status = x.Status,
                IsChecked = x.Status.Equals(nameof(EStatus.Active)) ? true : false,
            }).ToList();
            dataContainer.ItemsSource = containerDtos;
        }

        private async void Sua_Click(object sender, RoutedEventArgs e)
        {
            if (dataContainer.SelectedItem is ContainerDto selectedUser)
            {
                var container = await _containerRepository.GetByIdAsync(selectedUser.Id);
                if (container != null)
                {
                    LoadUserControl(new UpdateKhuVucHangControl(container, container.Id,_containerRepository, _vatLieuRepository));
                }
            }
        }

        private async void Xoa_Click(object sender, RoutedEventArgs e)
        {
            if (dataContainer.SelectedItem is ContainerDto selectedContainer)
            {
                var vatLieu = await _vatLieuRepository.ExistVatLieuByTypeContainerId(selectedContainer.Id);
                if (vatLieu)
                {
                    MessageBox.Show("Không được xóa khu vực này vì đang được sử dụng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                var container = await _containerRepository.GetByIdAsync(selectedContainer.Id);
                if (container != null)
                {
                    var result = MessageBox.Show("Bạn có thật sự muốn xóa khu vực này?", "Thông báo", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        _containerRepository.Delete(container);
                        await _containerRepository.SaveDbSetAsync();
                        MessageBox.Show("Xóa khu vực thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadData();
                    }                   
                }
                else
                {
                    MessageBox.Show("Xóa khu vực không thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                }               
            }
        }
        private async void CheckBox_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            // Xử lý khi người dùng nhấn vào checkbox
            var dataGridCell = sender as DataGridCell;
            if (dataGridCell != null)
            {
                var checkBox = dataGridCell.Content as CheckBox;
                if (checkBox != null)
                {
                    var containerDto = dataGridCell.DataContext as ContainerDto;
                    var vatLieu = await _vatLieuRepository.ExistVatLieuByTypeContainerId(containerDto!.Id);
                    if (vatLieu)
                    {
                        MessageBox.Show("Không thay đổi được trạng thái khu vực này vì đang được sử dụng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var container = await _containerRepository.GetByIdAsync(containerDto!.Id);
                    // Làm gì đó khi checkbox được click

                    bool isChecked = !containerDto.IsChecked;
                    if (isChecked)
                    {
                        container!.Status = nameof(EStatus.Active);
                        _containerRepository.Update(container);
                    }
                    else
                    {
                        container!.Status = nameof(EStatus.Inactive);
                        _containerRepository.Update(container);
                    }
                    await _containerRepository.SaveDbSetAsync();
                    await LoadData();
                    // Cập nhật trạng thái hoặc xử lý logic khác
                }
            }
        }
        private void LoadUserControl<T>(T userControl)
        {
            // Clear existing controls
            this.Content = userControl; // Load the new user control
        }

    }
}

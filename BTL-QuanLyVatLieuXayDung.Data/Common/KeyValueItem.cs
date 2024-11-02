using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_QuanLyVatLieuXayDung.Data.Common
{
    public class KeyValueItem
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Value; // Hiển thị giá trị trong ComboBox
        }
    }
}

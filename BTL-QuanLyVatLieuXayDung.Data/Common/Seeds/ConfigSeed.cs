using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_QuanLyVatLieuXayDung.Data.Common.Seeds
{
    public class ConfigSeed
    {
        public static Config[] GetConfigs()
        {
            return new[]
            {
                CreateConfig("SenderEmail", null, EGroupConfig.EmailServer),
                CreateConfig("SenderName", "Bộ phận một cửa", EGroupConfig.EmailServer),
                CreateConfig("SenderPort", "587", EGroupConfig.EmailServer),
                CreateConfig("SenderHost", "smtp.gmail.com", EGroupConfig.EmailServer),
                CreateConfig("SenderPassword", "1aK%2es%", EGroupConfig.EmailServer),
                CreateConfig("SysStatus", null, EGroupConfig.System),
                CreateConfig("SysMessage", null, EGroupConfig.System),
                CreateConfig("UniversityName", "Trường Đại học Mỏ - Địa chất", EGroupConfig.Information),
                CreateConfig("HocKy", "1", EGroupConfig.Information),
                CreateConfig("EmailContact", null, EGroupConfig.Information),
                CreateConfig("AddressContact", null, EGroupConfig.Information)
            };
        }

        private static Config CreateConfig(string paramName, string? paramValue, EGroupConfig group)
        {
            return new Config
            {
                ParamName = paramName,
                ParamValue = paramValue,
                Group = group.ToString(),
                Status = nameof(EStatus.Active)
            };
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BTL_QuanLyVatLieuXayDung.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumQuanityToTypeDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZG8E69B4HFNRHCGC4H");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGF4NK5030A9KMPED1");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGF5DRMN0EXQYGMX7A");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGFT4YBQ4KAM3J7SZW");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGG81Y6JJ3MJ64QMEW");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGHTJKYSYT9WF77VHZ");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGNP94YSZ34TSSFJWA");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGNXMB0P5J5318BEJM");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGP37KQXV57PE35JZQ");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGPA85CX1J3XN3QDRQ");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JC82TWZGX0P48W9VGNN5Z1VH");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "01JC82TWZGZZQ3F93MFNAHM7QF");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "Nhap",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.InsertData(
                table: "Config",
                columns: new[] { "Id", "Group", "ParamName", "ParamValue", "Status" },
                values: new object[,]
                {
                    { "01JD9DVWSP0ASDPK2MDPVJJG1Q", "EmailServer", "SenderPort", "587", "Active" },
                    { "01JD9DVWSP7DA1YWD6QXFM2D4D", "Information", "HocKy", "1", "Active" },
                    { "01JD9DVWSP9EC1A4YT0AMKVFRR", "EmailServer", "SenderHost", "smtp.gmail.com", "Active" },
                    { "01JD9DVWSPB5RFFCJRV7N8GQE6", "EmailServer", "SenderPassword", "1aK%2es%", "Active" },
                    { "01JD9DVWSPE28P934JAR9ZVSER", "Information", "UniversityName", "Trường Đại học Mỏ - Địa chất", "Active" },
                    { "01JD9DVWSPG08AGSFKFZ9HY1SK", "System", "SysMessage", null, "Active" },
                    { "01JD9DVWSPJD4X6KTA7AJHEYMY", "System", "SysStatus", null, "Active" },
                    { "01JD9DVWSPNEV5M78AR5GXRJ11", "EmailServer", "SenderName", "Bộ phận một cửa", "Active" },
                    { "01JD9DVWSPNKRKYJ63F1DWK1ZG", "Information", "AddressContact", null, "Active" },
                    { "01JD9DVWSPW2M8R27MAM4GXN0Z", "EmailServer", "SenderEmail", null, "Active" },
                    { "01JD9DVWSPW2RW0EACS3Q4KRCR", "Information", "EmailContact", null, "Active" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Address", "CCCD", "Email", "FullName", "Password", "Role", "Status", "UrlImage", "UserName" },
                values: new object[] { "01JD9DVWSPRCMJE7A2T6QZPCZS", "Adress", "1234567890", "Address@email.com", "Admin", "123", "Admin", "Active", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSP0ASDPK2MDPVJJG1Q");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSP7DA1YWD6QXFM2D4D");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSP9EC1A4YT0AMKVFRR");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSPB5RFFCJRV7N8GQE6");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSPE28P934JAR9ZVSER");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSPG08AGSFKFZ9HY1SK");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSPJD4X6KTA7AJHEYMY");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSPNEV5M78AR5GXRJ11");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSPNKRKYJ63F1DWK1ZG");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSPW2M8R27MAM4GXN0Z");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JD9DVWSPW2RW0EACS3Q4KRCR");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "01JD9DVWSPRCMJE7A2T6QZPCZS");

            migrationBuilder.AlterColumn<float>(
                name: "Quantity",
                table: "Nhap",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.InsertData(
                table: "Config",
                columns: new[] { "Id", "Group", "ParamName", "ParamValue", "Status" },
                values: new object[,]
                {
                    { "01JC82TWZG8E69B4HFNRHCGC4H", "System", "SysStatus", null, "Active" },
                    { "01JC82TWZGF4NK5030A9KMPED1", "System", "SysMessage", null, "Active" },
                    { "01JC82TWZGF5DRMN0EXQYGMX7A", "Information", "EmailContact", null, "Active" },
                    { "01JC82TWZGFT4YBQ4KAM3J7SZW", "Information", "UniversityName", "Trường Đại học Mỏ - Địa chất", "Active" },
                    { "01JC82TWZGG81Y6JJ3MJ64QMEW", "Information", "AddressContact", null, "Active" },
                    { "01JC82TWZGHTJKYSYT9WF77VHZ", "EmailServer", "SenderPassword", "1aK%2es%", "Active" },
                    { "01JC82TWZGNP94YSZ34TSSFJWA", "EmailServer", "SenderName", "Bộ phận một cửa", "Active" },
                    { "01JC82TWZGNXMB0P5J5318BEJM", "EmailServer", "SenderPort", "587", "Active" },
                    { "01JC82TWZGP37KQXV57PE35JZQ", "EmailServer", "SenderHost", "smtp.gmail.com", "Active" },
                    { "01JC82TWZGPA85CX1J3XN3QDRQ", "Information", "HocKy", "1", "Active" },
                    { "01JC82TWZGX0P48W9VGNN5Z1VH", "EmailServer", "SenderEmail", null, "Active" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Address", "CCCD", "Email", "FullName", "Password", "Role", "Status", "UrlImage", "UserName" },
                values: new object[] { "01JC82TWZGZZQ3F93MFNAHM7QF", "Adress", "1234567890", "Address@email.com", "Admin", "123", "Admin", "Active", null, "admin" });
        }
    }
}

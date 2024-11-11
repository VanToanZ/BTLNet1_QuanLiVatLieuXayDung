using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BTL_QuanLyVatLieuXayDung.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumParamVauleIsNullInTableConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "01JC0YZTBB78RPR00F0XS2DJ8Y");

            migrationBuilder.AlterColumn<string>(
                name: "ParamValue",
                table: "Config",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "ParamValue",
                table: "Config",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Address", "CCCD", "Email", "FullName", "Password", "Role", "Status", "UrlImage", "UserName" },
                values: new object[] { "01JC0YZTBB78RPR00F0XS2DJ8Y", "Adress", "1234567890", "Address@email.com", "Admin", "123", "Admin", "Active", null, "admin" });
        }
    }
}

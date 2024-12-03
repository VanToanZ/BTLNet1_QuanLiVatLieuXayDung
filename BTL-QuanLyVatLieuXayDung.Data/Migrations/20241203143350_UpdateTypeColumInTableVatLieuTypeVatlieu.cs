using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BTL_QuanLyVatLieuXayDung.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypeColumInTableVatLieuTypeVatlieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "VatLieu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameTypeVatLieu",
                table: "TypeVatLieu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "Config",
                columns: new[] { "Id", "Group", "ParamName", "ParamValue", "Status" },
                values: new object[,]
                {
                    { "01JE6F25NK1A6CW463G5NWGWWN", "Information", "EmailContact", null, "Active" },
                    { "01JE6F25NK1MDW7SK6SQJGQENC", "EmailServer", "SenderHost", "smtp.gmail.com", "Active" },
                    { "01JE6F25NK7XV0PA8TR3NVAHBB", "EmailServer", "SenderName", "Bộ phận một cửa", "Active" },
                    { "01JE6F25NKEN9C2GFP1Z0V2V3C", "Information", "AddressContact", null, "Active" },
                    { "01JE6F25NKG26TCEZSN8P76Q8J", "EmailServer", "SenderPassword", "1aK%2es%", "Active" },
                    { "01JE6F25NKN1YQRQKWEXPHM38H", "System", "SysMessage", null, "Active" },
                    { "01JE6F25NKNTEGP1GWBER7Q7TA", "Information", "HocKy", "1", "Active" },
                    { "01JE6F25NKSK4NQHGFY5BS413J", "Information", "UniversityName", "Trường Đại học Mỏ - Địa chất", "Active" },
                    { "01JE6F25NKT21PA1W4Q5YCY1Q0", "EmailServer", "SenderEmail", null, "Active" },
                    { "01JE6F25NKTBD8P0KME8VWN4A1", "System", "SysStatus", null, "Active" },
                    { "01JE6F25NKVMNVZES3KDH489HM", "EmailServer", "SenderPort", "587", "Active" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Address", "CCCD", "Email", "FullName", "Password", "Role", "Status", "UrlImage", "UserName" },
                values: new object[] { "01JE6F25NKR5D5RDHCS6VGDB7F", "Adress", "1234567890", "Address@email.com", "Admin", "123", "Admin", "Active", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NK1A6CW463G5NWGWWN");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NK1MDW7SK6SQJGQENC");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NK7XV0PA8TR3NVAHBB");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NKEN9C2GFP1Z0V2V3C");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NKG26TCEZSN8P76Q8J");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NKN1YQRQKWEXPHM38H");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NKNTEGP1GWBER7Q7TA");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NKSK4NQHGFY5BS413J");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NKT21PA1W4Q5YCY1Q0");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NKTBD8P0KME8VWN4A1");

            migrationBuilder.DeleteData(
                table: "Config",
                keyColumn: "Id",
                keyValue: "01JE6F25NKVMNVZES3KDH489HM");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "01JE6F25NKR5D5RDHCS6VGDB7F");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "VatLieu",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameTypeVatLieu",
                table: "TypeVatLieu",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

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
    }
}

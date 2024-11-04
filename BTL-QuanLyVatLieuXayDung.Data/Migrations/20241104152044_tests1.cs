using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_QuanLyVatLieuXayDung.Data.Migrations
{
    /// <inheritdoc />
    public partial class tests1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "01JBPKWZKKWHK6WA0VD90KYVPC");

            migrationBuilder.AlterColumn<string>(
                name: "UpdateBy",
                table: "TypeVatLieu",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionContainer",
                table: "Container",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Address", "CCCD", "Email", "FullName", "Password", "Role", "Status", "UrlImage", "UserName" },
                values: new object[] { "01JBVW765HM17YYHJ3GJ0N57G5", "Adress", "1234567890", "Address@email.com", "Admin", "123", "Admin", "Active", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "01JBVW765HM17YYHJ3GJ0N57G5");

            migrationBuilder.AlterColumn<string>(
                name: "UpdateBy",
                table: "TypeVatLieu",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionContainer",
                table: "Container",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Address", "CCCD", "Email", "FullName", "Password", "Role", "Status", "UrlImage", "UserName" },
                values: new object[] { "01JBPKWZKKWHK6WA0VD90KYVPC", "Adress", "1234567890", "Address@email.com", "Admin", "123", "Admin", "Active", null, "admin" });
        }
    }
}

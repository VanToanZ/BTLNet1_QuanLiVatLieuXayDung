using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BTL_QuanLyVatLieuXayDung.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParamName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ParamValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Group = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Container",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameContainer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodeContainer = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UrlImage = table.Column<string>(type: "text", nullable: false),
                    DescriptionContainer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Container", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodeHoaDon = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AddressNhan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DescriptionHoaDon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SoDienThoai = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    CreateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalMoney = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeVatLieu",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameTypeVatLieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodeTypeVatLieu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UrlImage = table.Column<string>(type: "text", nullable: false),
                    CreateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UpdateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeVatLieu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CCCD = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UrlImage = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VatLieu",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameVatLieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodeVatLieu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TypeVatLieuId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContainerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UrlImage = table.Column<string>(type: "text", nullable: true),
                    CreateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatLieu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VatLieu_Container_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Container",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VatLieu_TypeVatLieu_TypeVatLieuId",
                        column: x => x.TypeVatLieuId,
                        principalTable: "TypeVatLieu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailHoaDon",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quanity = table.Column<double>(type: "float", nullable: false),
                    HoaDonId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VatLieuId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalMoney = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailHoaDon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailHoaDon_HoaDon_HoaDonId",
                        column: x => x.HoaDonId,
                        principalTable: "HoaDon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailHoaDon_VatLieu_VatLieuId",
                        column: x => x.VatLieuId,
                        principalTable: "VatLieu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nhap",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    VatLieuId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    TotalMoney = table.Column<double>(type: "float", nullable: false),
                    CreateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nhap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nhap_VatLieu_VatLieuId",
                        column: x => x.VatLieuId,
                        principalTable: "VatLieu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Config",
                columns: new[] { "Id", "Group", "ParamName", "ParamValue", "Status" },
                values: new object[,]
                {
                    { "01JEB667QZ0MXNKYJKV9VQQFR0", "Information", "UniversityName", "Trường Đại học Mỏ - Địa chất", "Active" },
                    { "01JEB667QZ34FN00X1MQSHBK0T", "System", "SysMessage", null, "Active" },
                    { "01JEB667QZ46DE99HS5P863FCB", "EmailServer", "SenderPassword", "1aK%2es%", "Active" },
                    { "01JEB667QZ61F97HCF7N4KWW4R", "Information", "AddressContact", null, "Active" },
                    { "01JEB667QZ8WAHRTTA1KZBBKT7", "EmailServer", "SenderEmail", null, "Active" },
                    { "01JEB667QZBEM9KFC88EZXFPHE", "EmailServer", "SenderHost", "smtp.gmail.com", "Active" },
                    { "01JEB667QZC4XE831B0VG1QBNH", "EmailServer", "SenderPort", "587", "Active" },
                    { "01JEB667QZHRGZTDSQ3RMDXMY6", "System", "SysStatus", null, "Active" },
                    { "01JEB667QZP80NWWXXGXXQNTE9", "Information", "EmailContact", null, "Active" },
                    { "01JEB667QZXSDFGKHPKD01S942", "EmailServer", "SenderName", "Bộ phận một cửa", "Active" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Address", "CCCD", "Email", "FullName", "Password", "Role", "Status", "UrlImage", "UserName" },
                values: new object[] { "01JEB667QZP31VVAGQXK7X1RWC", "Adress", "1234567890", "Address@email.com", "Admin", "123", "Admin", "Active", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_DetailHoaDon_HoaDonId",
                table: "DetailHoaDon",
                column: "HoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailHoaDon_VatLieuId",
                table: "DetailHoaDon",
                column: "VatLieuId");

            migrationBuilder.CreateIndex(
                name: "IX_Nhap_VatLieuId",
                table: "Nhap",
                column: "VatLieuId");

            migrationBuilder.CreateIndex(
                name: "IX_VatLieu_ContainerId",
                table: "VatLieu",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_VatLieu_TypeVatLieuId",
                table: "VatLieu",
                column: "TypeVatLieuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "DetailHoaDon");

            migrationBuilder.DropTable(
                name: "Nhap");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "VatLieu");

            migrationBuilder.DropTable(
                name: "Container");

            migrationBuilder.DropTable(
                name: "TypeVatLieu");
        }
    }
}

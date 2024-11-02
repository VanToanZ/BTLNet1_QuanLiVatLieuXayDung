﻿// <auto-generated />
using System;
using BTL_QuanLyVatLieuXayDung.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BTL_QuanLyVatLieuXayDung.Data.Migrations
{
    [DbContext(typeof(QuanLyVatLieuXayDungDbContext))]
    partial class QuanLyVatLieuXayDungDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.Config", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("ParamName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("ParamValue")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.ToTable("Config");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.Container", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CodeContainer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("DescriptionContainer")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar");

                    b.Property<string>("NameContainer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("UrlImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Container");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.DetailHoaDon", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("HoaDonId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Quanity")
                        .HasColumnType("real");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<double>("TotalMoney")
                        .HasColumnType("float");

                    b.Property<string>("VatLieuId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("HoaDonId");

                    b.HasIndex("VatLieuId");

                    b.ToTable("DetailHoaDon");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.HoaDon", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AddressNhan")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar");

                    b.Property<string>("CodeHoaDon")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("DescriptionHoaDon")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<double>("TotalMoney")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("HoaDon");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.Nhap", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<float>("Quantity")
                        .HasColumnType("real");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<double>("TotalMoney")
                        .HasColumnType("float");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("VatLieuId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("VatLieuId");

                    b.ToTable("Nhap");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.TypeVatLieu", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CodeTypeVatLieu")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("NameTypeVatLieu")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("UpdateBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("UrlImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TypeVatLieu");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.VatLieu", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CodeVatLieu")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("ContainerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("NameVatLieu")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("TypeVatLieuId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("UrlImage")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ContainerId");

                    b.HasIndex("TypeVatLieuId");

                    b.ToTable("VatLieu");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("UrlImage")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = "01JBPKWZKKWHK6WA0VD90KYVPC",
                            Address = "Adress",
                            CCCD = "1234567890",
                            Email = "Address@email.com",
                            FullName = "Admin",
                            Password = "123",
                            Role = "Admin",
                            Status = "Active",
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.DetailHoaDon", b =>
                {
                    b.HasOne("BTL_QuanLyVatLieuXayDung.Data.Models.HoaDon", "HoaDonForeignKey")
                        .WithMany("DetailHoaDons")
                        .HasForeignKey("HoaDonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BTL_QuanLyVatLieuXayDung.Data.Models.VatLieu", "VatLieuForeignKey")
                        .WithMany("DetailHoaDons")
                        .HasForeignKey("VatLieuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HoaDonForeignKey");

                    b.Navigation("VatLieuForeignKey");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.Nhap", b =>
                {
                    b.HasOne("BTL_QuanLyVatLieuXayDung.Data.Models.VatLieu", "VatLieuForeignKey")
                        .WithMany("Nhaps")
                        .HasForeignKey("VatLieuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VatLieuForeignKey");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.VatLieu", b =>
                {
                    b.HasOne("BTL_QuanLyVatLieuXayDung.Data.Models.Container", "ContainerForeignKey")
                        .WithMany("VatLieus")
                        .HasForeignKey("ContainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BTL_QuanLyVatLieuXayDung.Data.Models.TypeVatLieu", "TypeVatLieuForeignKey")
                        .WithMany("VatLieus")
                        .HasForeignKey("TypeVatLieuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContainerForeignKey");

                    b.Navigation("TypeVatLieuForeignKey");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.Container", b =>
                {
                    b.Navigation("VatLieus");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.HoaDon", b =>
                {
                    b.Navigation("DetailHoaDons");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.TypeVatLieu", b =>
                {
                    b.Navigation("VatLieus");
                });

            modelBuilder.Entity("BTL_QuanLyVatLieuXayDung.Data.Models.VatLieu", b =>
                {
                    b.Navigation("DetailHoaDons");

                    b.Navigation("Nhaps");
                });
#pragma warning restore 612, 618
        }
    }
}

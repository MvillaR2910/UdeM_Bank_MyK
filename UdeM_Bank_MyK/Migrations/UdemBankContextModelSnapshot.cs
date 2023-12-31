﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UdeM_Bank_MyK;

#nullable disable

namespace UdeM_Bank_MyK.Migrations
{
    [DbContext(typeof(UdemBankContext))]
    partial class UdemBankContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("ClienteGrupoAhorro", b =>
                {
                    b.Property<int>("GruposDeAhorrosIdGrupoAhorro")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsuariosIdCliente")
                        .HasColumnType("INTEGER");

                    b.HasKey("GruposDeAhorrosIdGrupoAhorro", "UsuariosIdCliente");

                    b.HasIndex("UsuariosIdCliente");

                    b.ToTable("ClienteGrupoAhorro");
                });

            modelBuilder.Entity("UdeM_Bank_MyK.Cliente", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ComisionReducida")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NroGruposPertenecientes")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Saldo")
                        .HasColumnType("REAL");

                    b.HasKey("IdCliente");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("UdeM_Bank_MyK.GrupoAhorro", b =>
                {
                    b.Property<int>("IdGrupoAhorro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Capital")
                        .HasColumnType("REAL");

                    b.Property<string>("NombreGrupo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdGrupoAhorro");

                    b.ToTable("GruposAhorros");
                });

            modelBuilder.Entity("UdeM_Bank_MyK.GrupoAhorroXCliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("AporteCliente")
                        .HasColumnType("REAL");

                    b.Property<int>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdGrupoAhorro")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("IdCliente");

                    b.HasIndex("IdGrupoAhorro");

                    b.ToTable("GrupoAhorroXCliente");
                });

            modelBuilder.Entity("UdeM_Bank_MyK.MovimientosGrupoAhorroXCliente", b =>
                {
                    b.Property<int>("IdMovimiento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<string>("Hora")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdGrupoDestinatario")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Monto")
                        .HasColumnType("REAL");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdMovimiento");

                    b.HasIndex("IdCliente");

                    b.ToTable("Movimientos");
                });

            modelBuilder.Entity("UdeM_Bank_MyK.Prestamo", b =>
                {
                    b.Property<int>("IdPrestamo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Cantidad")
                        .HasColumnType("REAL");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdGrupoAhorro")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Interes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MesesAPagar")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdPrestamo");

                    b.HasIndex("IdCliente");

                    b.HasIndex("IdGrupoAhorro");

                    b.ToTable("Prestamos");
                });

            modelBuilder.Entity("ClienteGrupoAhorro", b =>
                {
                    b.HasOne("UdeM_Bank_MyK.GrupoAhorro", null)
                        .WithMany()
                        .HasForeignKey("GruposDeAhorrosIdGrupoAhorro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UdeM_Bank_MyK.Cliente", null)
                        .WithMany()
                        .HasForeignKey("UsuariosIdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UdeM_Bank_MyK.GrupoAhorroXCliente", b =>
                {
                    b.HasOne("UdeM_Bank_MyK.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UdeM_Bank_MyK.GrupoAhorro", "GrupoAhorro")
                        .WithMany()
                        .HasForeignKey("IdGrupoAhorro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("GrupoAhorro");
                });

            modelBuilder.Entity("UdeM_Bank_MyK.MovimientosGrupoAhorroXCliente", b =>
                {
                    b.HasOne("UdeM_Bank_MyK.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("UdeM_Bank_MyK.Prestamo", b =>
                {
                    b.HasOne("UdeM_Bank_MyK.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UdeM_Bank_MyK.GrupoAhorro", "GrupoAhorro")
                        .WithMany()
                        .HasForeignKey("IdGrupoAhorro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("GrupoAhorro");
                });
#pragma warning restore 612, 618
        }
    }
}

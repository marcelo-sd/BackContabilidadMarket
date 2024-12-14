﻿// <auto-generated />
using System;
using ContabilidaMarket.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContabilidaMarket.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241105152022_anadiForingKey")]
    partial class anadiForingKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ContabilidaMarket.Models.EstadoPedido", b =>
                {
                    b.Property<int>("IdEstado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEstado"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEstado");

                    b.ToTable("EstadoPedidos");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPedido"));

                    b.Property<int>("EstadoPedidoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.HasKey("IdPedido");

                    b.HasIndex("EstadoPedidoId");

                    b.HasIndex("idUsuario");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.PedidoProducto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdPedido")
                        .HasColumnType("int");

                    b.Property<int>("IdProducto")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPedido");

                    b.HasIndex("IdProducto");

                    b.ToTable("PedidoProductos");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.Producto", b =>
                {
                    b.Property<int>("IdProductos")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProductos"));

                    b.Property<string>("CodigoBarra")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Precioo")
                        .HasColumnType("float");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("IdProductos");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.Rol", b =>
                {
                    b.Property<int>("IdRol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRol"));

                    b.Property<string>("RolNombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdRol");

                    b.ToTable("Rols");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rol_id")
                        .HasColumnType("int");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUsuario");

                    b.HasIndex("Rol_id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.Pedido", b =>
                {
                    b.HasOne("ContabilidaMarket.Models.EstadoPedido", "EstadoPedido")
                        .WithMany("Pedidos")
                        .HasForeignKey("EstadoPedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContabilidaMarket.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EstadoPedido");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.PedidoProducto", b =>
                {
                    b.HasOne("ContabilidaMarket.Models.Pedido", "Pedido")
                        .WithMany("PedidoProductos")
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContabilidaMarket.Models.Producto", "Producto")
                        .WithMany("PedidoProductos")
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.Usuario", b =>
                {
                    b.HasOne("ContabilidaMarket.Models.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("Rol_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.EstadoPedido", b =>
                {
                    b.Navigation("Pedidos");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.Pedido", b =>
                {
                    b.Navigation("PedidoProductos");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.Producto", b =>
                {
                    b.Navigation("PedidoProductos");
                });

            modelBuilder.Entity("ContabilidaMarket.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });
#pragma warning restore 612, 618
        }
    }
}

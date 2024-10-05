﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Exata.Repository.Context;

#nullable disable

namespace Exata.Repository.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20240620185959_TabPerfilSecaoRemoveDatas")]
    partial class TabPerfilSecaoRemoveDatas
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Exata.Domain.Entities.Campo", b =>
                {
                    b.Property<string>("TabelaID")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CampoID")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool>("Ordena")
                        .HasColumnType("bit");

                    b.Property<bool>("Pesquisa")
                        .HasColumnType("bit");

                    b.HasKey("TabelaID", "CampoID");

                    b.ToTable("Campo", (string)null);
                });

            modelBuilder.Entity("Exata.Domain.Entities.Contato", b =>
                {
                    b.Property<string>("TelefoneID")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("UserAlteracao")
                        .HasColumnType("int");

                    b.Property<int?>("UserCadastro")
                        .HasColumnType("int");

                    b.HasKey("TelefoneID");

                    b.ToTable("Contato");
                });

            modelBuilder.Entity("Exata.Domain.Entities.ContatoTipo", b =>
                {
                    b.Property<string>("TelefoneID")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("TipoContatoID")
                        .HasColumnType("int");

                    b.HasKey("TelefoneID", "TipoContatoID");

                    b.HasIndex("TipoContatoID");

                    b.ToTable("ContatoTipo", (string)null);
                });

            modelBuilder.Entity("Exata.Domain.Entities.Perfil", b =>
                {
                    b.Property<int>("PerfilID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PerfilID"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("UserAlteracao")
                        .HasColumnType("int");

                    b.Property<int?>("UserCadastro")
                        .HasColumnType("int");

                    b.HasKey("PerfilID");

                    b.ToTable("Perfil");
                });

            modelBuilder.Entity("Exata.Domain.Entities.PerfilSecao", b =>
                {
                    b.Property<int>("PerfilID")
                        .HasColumnType("int");

                    b.Property<int>("SecaoID")
                        .HasColumnType("int");

                    b.HasKey("PerfilID", "SecaoID");

                    b.HasIndex("SecaoID");

                    b.ToTable("PerfilSecao", (string)null);
                });

            modelBuilder.Entity("Exata.Domain.Entities.Secao", b =>
                {
                    b.Property<int>("SecaoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SecaoID"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TelefoneID")
                        .HasColumnType("int");

                    b.Property<int?>("UserAlteracao")
                        .HasColumnType("int");

                    b.Property<int?>("UserCadastro")
                        .HasColumnType("int");

                    b.HasKey("SecaoID");

                    b.HasIndex("TelefoneID");

                    b.ToTable("Secao");
                });

            modelBuilder.Entity("Exata.Domain.Entities.Telefones", b =>
                {
                    b.Property<int>("TelefoneID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TelefoneID"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int?>("UserAlteracao")
                        .HasColumnType("int");

                    b.Property<int?>("UserCadastro")
                        .HasColumnType("int");

                    b.HasKey("TelefoneID");

                    b.HasIndex("Telefone")
                        .IsUnique();

                    b.ToTable("Telefone");
                });

            modelBuilder.Entity("Exata.Domain.Entities.TipoContato", b =>
                {
                    b.Property<int>("TipoContatoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TipoContatoID"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("UserAlteracao")
                        .HasColumnType("int");

                    b.Property<int?>("UserCadastro")
                        .HasColumnType("int");

                    b.HasKey("TipoContatoID");

                    b.ToTable("TipoContato");
                });

            modelBuilder.Entity("Exata.Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("UsuarioID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioID"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("PerfilID")
                        .HasColumnType("int");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("UserAlteracao")
                        .HasColumnType("int");

                    b.Property<int?>("UserCadastro")
                        .HasColumnType("int");

                    b.HasKey("UsuarioID");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("PerfilID");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("Exata.Domain.Entities.ContatoTipo", b =>
                {
                    b.HasOne("Exata.Domain.Entities.Contato", "Contato")
                        .WithMany("ContatoTipo")
                        .HasForeignKey("TelefoneID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Exata.Domain.Entities.TipoContato", "TipoContato")
                        .WithMany("ContatoTipo")
                        .HasForeignKey("TipoContatoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contato");

                    b.Navigation("TipoContato");
                });

            modelBuilder.Entity("Exata.Domain.Entities.PerfilSecao", b =>
                {
                    b.HasOne("Exata.Domain.Entities.Perfil", "Perfil")
                        .WithMany("PerfilSecao")
                        .HasForeignKey("PerfilID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Exata.Domain.Entities.Secao", "Secao")
                        .WithMany("PerfilSecao")
                        .HasForeignKey("SecaoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Perfil");

                    b.Navigation("Secao");
                });

            modelBuilder.Entity("Exata.Domain.Entities.Secao", b =>
                {
                    b.HasOne("Exata.Domain.Entities.Telefones", "Telefone")
                        .WithMany("Secao")
                        .HasForeignKey("TelefoneID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Telefone");
                });

            modelBuilder.Entity("Exata.Domain.Entities.Usuario", b =>
                {
                    b.HasOne("Exata.Domain.Entities.Perfil", "Perfil")
                        .WithMany("Usuarios")
                        .HasForeignKey("PerfilID");

                    b.Navigation("Perfil");
                });

            modelBuilder.Entity("Exata.Domain.Entities.Contato", b =>
                {
                    b.Navigation("ContatoTipo");
                });

            modelBuilder.Entity("Exata.Domain.Entities.Perfil", b =>
                {
                    b.Navigation("PerfilSecao");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Exata.Domain.Entities.Secao", b =>
                {
                    b.Navigation("PerfilSecao");
                });

            modelBuilder.Entity("Exata.Domain.Entities.Telefones", b =>
                {
                    b.Navigation("Secao");
                });

            modelBuilder.Entity("Exata.Domain.Entities.TipoContato", b =>
                {
                    b.Navigation("ContatoTipo");
                });
#pragma warning restore 612, 618
        }
    }
}

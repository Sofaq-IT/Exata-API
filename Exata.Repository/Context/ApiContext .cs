using Microsoft.EntityFrameworkCore;
using Exata.Domain.Entities;

namespace Exata.Repository.Context;

public partial class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PerfilSecao>()
            .ToTable("PerfilSecao")
            .HasKey(c => new { c.PerfilID, c.SecaoID });

        modelBuilder.Entity<ContatoTipo>()
            .ToTable("ContatoTipo")
            .HasKey(c => new { c.TelefoneID, c.TipoContatoID });

        modelBuilder.Entity<Campo>()
            .ToTable("Campo")
            .HasKey(c => new { c.TabelaID, c.CampoID });

        modelBuilder.Entity<Usuario>()
            .HasOne(t => t.Perfil)
            .WithMany(b => b.Usuarios)
            .HasForeignKey(t => new { t.PerfilID })
            .HasPrincipalKey(b => new { b.PerfilID });

        modelBuilder.Entity<Usuario>()
            .HasIndex(t => t.Login)
            .IsUnique();

        modelBuilder.Entity<Secao>()
            .HasOne(t => t.Telefone)
            .WithMany(b => b.Secao)
            .HasForeignKey(t => new { t.TelefoneID })
            .HasPrincipalKey(b => new { b.TelefoneID });

        modelBuilder.Entity<PerfilSecao>()
            .HasOne(t => t.Perfil)
            .WithMany(b => b.PerfilSecao)
            .HasForeignKey(t => new { t.PerfilID })
            .HasPrincipalKey(b => new { b.PerfilID });

        modelBuilder.Entity<PerfilSecao>()
            .HasOne(t => t.Secao)
            .WithMany(b => b.PerfilSecao)
            .HasForeignKey(t => new { t.SecaoID })
            .HasPrincipalKey(b => new { b.SecaoID });

        modelBuilder.Entity<ContatoTipo>()
            .HasOne(t => t.TipoContato)
            .WithMany(b => b.ContatoTipo)
            .HasForeignKey(t => new { t.TipoContatoID })
            .HasPrincipalKey(b => new { b.TipoContatoID });

        modelBuilder.Entity<ContatoTipo>()
            .HasOne(t => t.Contato)
            .WithMany(b => b.ContatoTipo)
            .HasForeignKey(t => new { t.TelefoneID })
            .HasPrincipalKey(b => new { b.TelefoneID });

        modelBuilder.Entity<Telefones>()
            .HasIndex(t => t.Telefone)
            .IsUnique();

        //Cada Tabela tem que ter o vinculo do usuário que criou ou Alterou
        //modelBuilder.Entity<Usuario>()
        //.HasOne(t => t.UserCadContato)
        //.WithMany(b => b.LUserCad)
        //.HasForeignKey(t => new { t.UserCad })
        //.HasPrincipalKey(b => new { b.UsuarioID });

        //modelBuilder.Entity<Usuario>()
        //.HasOne(t => t.UserAlt)
        //.WithMany(b => b.LUserAlt)
        //.HasForeignKey(t => new { t.UserAlt })
        //.HasPrincipalKey(b => new { b.UsuarioID });
    }

    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Perfil> Perfil { get; set; }
    public DbSet<Telefones> Telefone { get; set; }
    public DbSet<Secao> Secao { get; set; }
    public DbSet<PerfilSecao> PerfilSecao { get; set; }
    public DbSet<Contato> Contato { get; set; }
    public DbSet<TipoContato> TipoContato { get; set; }
    public DbSet<ContatoTipo> ContatoTipo { get; set; }
    public DbSet<Campo> Campo { get; set; }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Exata.Domain.Entities;

namespace Exata.Repository.Context;

public class ApiContext : IdentityDbContext<ApplicationUser>
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region "Inserção de Chave Primária com mais de um campo"

        modelBuilder.Entity<Campo>()
            .ToTable("Campo")
            .HasKey(c => new { c.TabelaID, c.CampoID });

        modelBuilder.Entity<ControllerAction>()
            .ToTable("ControllerAction")
            .HasKey(c => new { c.Controller, c.Metodo, c.Action });

        modelBuilder.Entity<LogRequisicao>()
            .ToTable("LogRequisicoes")
            .HasKey(c => new { c.Data, c.UsuarioID, c.Metodo, c.Controller, c.Action });

        modelBuilder.Entity<PerfilControllerAction>()
            .ToTable("PerfilControllerAction")
            .HasKey(c => new { c.ControllerActionID, c.PerfilID });

        #endregion

        #region "Inserção dos vinculos entre tabelas (Chaves Secundárias - 1 x 1)"
        
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(e => e.UsuarioAvatar)
            .WithOne(e => e.Usuario)
            .HasForeignKey<UsuarioAvatar>(e => e.UsuarioID)
            .IsRequired();

        #endregion

        #region "Inserção dos vinculos entre tabelas (Chaves Secundárias - 1 x X)"

        #region "ApplicationUser"

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(t => t.Perfil)
            .WithMany(b => b.Users)
            .HasForeignKey(t => new { t.PerfilID })
            .HasPrincipalKey(b => new { b.PerfilID });

        #endregion
                
        #region "Perfil"

        modelBuilder.Entity<Perfil>()
            .HasOne(t => t.UsuarioPerfilCriacao)
            .WithMany(b => b.PerfilCriacao)
            .HasForeignKey(t => new { t.UserCadastro })
            .HasPrincipalKey(b => new { b.Id });

        modelBuilder.Entity<Perfil>()
            .HasOne(t => t.UsuarioPerfilAlteracao)
            .WithMany(b => b.PerfilAlteracao)
            .HasForeignKey(t => new { t.UserAlteracao })
            .HasPrincipalKey(b => new { b.Id });

        #endregion

        #region "PerfilControllerAction"

        modelBuilder.Entity<PerfilControllerAction>()
            .HasOne(t => t.Perfil)
            .WithMany(b => b.PerfilControllerAction)
            .HasForeignKey(t => new { t.PerfilID })
            .HasPrincipalKey(b => new { b.PerfilID });

        modelBuilder.Entity<PerfilControllerAction>()
            .HasOne(t => t.ControllerAction)
            .WithMany(b => b.PerfilControllerAction)
            .HasForeignKey(t => new { t.ControllerActionID })
            .HasPrincipalKey(b => new { b.ControllerActionID });

        #endregion

        #endregion

        //Retira todas as exclusões em cascatas dos relacionamentos
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }
    }

    public DbSet<Campo> Campo { get; set; }
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<ControllerAction> ControllerAction { get; set; }
    public DbSet<LogRequisicao> LogRequisicoes { get; set; }
    public DbSet<Perfil> Perfil { get; set; }
    public DbSet<PerfilControllerAction> PerfilControllerAction { get; set; }
    public DbSet<UsuarioAvatar> UsuarioAvatar { get; set; }
}
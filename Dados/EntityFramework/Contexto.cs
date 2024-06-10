using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Dados.EntityFramework.Configuration;

namespace WebApplication1.Dados.EntityFramework
{
    public class Contexto : DbContext
    {
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Grupos> Grupos { get; set; }
        public DbSet<ParticipantesGrupo> ParticipantesGrupo { get; set; }

        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.UsuariosID);
                entity.Property(e => e.UsuariosID).UseIdentityColumn();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.ToTable("Usuarios");
            });

            modelBuilder.Entity<Grupos>(entity =>
            {
                entity.HasKey(e => e.GruposID);
                entity.ToTable("Grupos");
            });

            modelBuilder.Entity<ParticipantesGrupo>(entity =>
            {
                entity.HasKey(e => new { e.ID_Grupo, e.ID_Participante });
                entity.ToTable("ParticipantesGrupo");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data source=201.62.57.93,1434; 
                                           Database=BD038216; 
                                           User ID=RA038216; 
                                           Password=038216; 
                                           TrustServerCertificate=True");
        }
    }
}
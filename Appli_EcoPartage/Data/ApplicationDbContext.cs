using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace Appli_EcoPartage.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users, IdentityRole<int>, int>
    {
        public DbSet<Annonces> Annonces { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<AnnoncesTags> AnnoncesTags { get; set; }
        public DbSet<GeographicalSector> GeographicalSectors { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Annonces>()
                .HasKey(t => t.IdAnnonce);

            // Configuration des relations pour Comments
            modelBuilder.Entity<Comments>()
                .HasOne(c => c.Giver)
                .WithMany(u => u.CommentsGiven)
                .HasForeignKey(c => c.IdUserGiven)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comments>()
                .HasOne(c => c.Recipient)
                .WithMany(u => u.CommentsRecived)
                .HasForeignKey(c => c.IdUserRecipient)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration des relations pour AnnoncesTags
            modelBuilder.Entity<AnnoncesTags>()
            .HasKey(t => new { t.IdAnnonce, t.IdTag });

            // Configuration des relations pour GeographicalSector
            modelBuilder.Entity<GeographicalSector>()
                .HasKey(gs => new { gs.IdAnnonce, gs.IdGeographicalSector });

            modelBuilder.Entity<GeographicalSector>()
                .HasOne(gs => gs.Annonce)
                .WithMany(a => a.GeographicalSectors)
                .HasForeignKey(gs => gs.IdAnnonce)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration des relations pour Transactions
            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.UserGiver)
                .WithMany()
                .HasForeignKey(t => t.UserIdGiver)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.UserRecipient)
                .WithMany()
                .HasForeignKey(t => t.UserIdRecipient)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transactions>()
            .HasOne(t => t.Annonce)
            .WithMany()
            .HasForeignKey(t => t.IdAnnonce)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.AnnoncePoints)
                .WithMany()
                .HasForeignKey(t => t.AnnoncePoint)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tags>()
                .HasKey(t => t.IdTag);

            modelBuilder.Entity<Users>()
               .HasMany(u => u.CommentsGiven)
               .WithOne(c => c.Giver)
               .HasForeignKey(c => c.IdUserGiven)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
                .HasMany(u => u.CommentsRecived)
                .WithOne(c => c.Recipient)
                .HasForeignKey(c => c.IdUserRecipient)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

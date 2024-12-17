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
        public DbSet<AnnoncesGeoSector> AnnoncesGeoSectors { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Annonces>()
                .HasKey(t => t.IdAnnonce);


            modelBuilder.Entity<Tags>()
                .HasKey(t => t.IdTag);

            modelBuilder.Entity<GeographicalSector>()
                .HasKey(t => t.IdGeographicalSector);

            // Configuration des relations pour AnnoncesTags
            modelBuilder.Entity<AnnoncesTags>()
            .HasKey(t => new { t.IdAnnonce, t.IdTag, t.IdAnnonceTag });

            // Configuration des relations pour AnnonceGeoSector
            modelBuilder.Entity<AnnoncesGeoSector>()
            .HasKey(t => new { t.IdAnnonce, t.IdGeographicalSector, t.IdAnnoncesGeoSector });

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

            // Configuration des relations pour Annonces
            modelBuilder.Entity<Annonces>()
                .HasOne(a => a.User)
                .WithMany(u => u.MyAnnonces)
                .HasForeignKey(a => a.IdUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Annonces>()
                .HasMany(a => a.AnnoncesTags)
                .WithOne(at => at.Annonce)
                .HasForeignKey(at => at.IdAnnonce)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Annonces>()
                .HasMany(a => a.AnnoncesGeoSectors)
                .WithOne(ag => ag.Annonce)
                .HasForeignKey(ag => ag.IdAnnonce)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration des relations pour Transactions
            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.UserSeller)
                .WithMany(u => u.TransactionsSeller)
                .HasForeignKey(t => t.UserIdSeller)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.UserBuyer)
                .WithMany(u => u.TransactionsBuyer)
                .HasForeignKey(t => t.UserIdBuyer)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transactions>()
            .HasOne(t => t.Annonce)
            .WithMany()
            .HasForeignKey(t => t.IdAnnonce)
            .OnDelete(DeleteBehavior.Restrict);

            // Configuration des relations pour Users
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

            modelBuilder.Entity<Users>()
                .HasMany(u => u.MyAnnonces)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.IdUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ContactMessage>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.ContactMessages)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

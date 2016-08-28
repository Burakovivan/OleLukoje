using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace OleLukoje.Models
{
    public class OleLukojeContext : DbContext
    {
        public OleLukojeContext()
            : base("DefaultConnection")
        { }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Application> Applications { get; set; }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ad>()
                .HasMany(a => a.Files)
                .WithRequired(f => f.Ad)
                .HasForeignKey(f => f.AdId);

            modelBuilder.Entity<Ad>()
                .HasMany(a => a.Categories)
                .WithMany(c => c.Ads)
                .Map(t => t.MapLeftKey("AdId")
                .MapRightKey("CategoriesId")
                .ToTable("AdCategories"));

            modelBuilder.Entity<Ad>()
                .HasMany(a => a.Messages)
                .WithRequired(m => m.Ad)
                .HasForeignKey(m => m.AdId);

            modelBuilder.Entity<UserProfile>()
                .HasMany(u => u.Ads)
                .WithRequired(a => a.UserProfile)
                .HasForeignKey(a => a.UserId);

            base.OnModelCreating(modelBuilder);
        }*/
    }
}
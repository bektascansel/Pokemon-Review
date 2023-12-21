using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        { 
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //many to many ayarları 
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId });
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(pc => pc.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(pc => pc.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
               .HasOne(pc => pc.Category)
               .WithMany(pc => pc.PokemonCategories)
               .HasForeignKey(pc => pc.CategoryId);


            modelBuilder.Entity<PokemonOwner>()
                .HasKey(pc => new { pc.PokemonId, pc.OwnerId });
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(pc => pc.Pokemon)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(pc => pc.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
               .HasOne(pc => pc.Owner)
               .WithMany(pc => pc.PokemonOwners)
               .HasForeignKey(pc => pc.OwnerId);





        }

    }
}

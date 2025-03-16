using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;
using System.Reflection.Metadata;

namespace OneBeyondApi.DataAccess
{
    public class LibraryContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "AuthorDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(e => e.Reservations)
                .WithOne()
                .HasForeignKey(e => e.BookId)
                .IsRequired();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookStock> Catalogue { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
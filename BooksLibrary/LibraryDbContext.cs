using BooksLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Genre> Genres => Set<Genre>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBook(modelBuilder);
            ConfigureAuthor(modelBuilder);
            ConfigureGenre(modelBuilder);
        }

        private void ConfigureBook(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity => {
                // устанавливаем первичный ключ как Id модели
                entity.HasKey(book => book.Id);

                // настраиваем все поля модели
                entity.Property(book => book.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(book => book.ISBN)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(book => book.PublishYear)
                    .IsRequired();
                entity.Property(book => book.QuantityInStock)
                    .IsRequired();

                // relationship с моделью автора
                entity.HasOne(book => book.Author)
                    .WithMany(author => author.Books)
                    .HasForeignKey(book => book.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                // relationship с моделью жанра
                entity.HasOne(book => book.Genre)
                    .WithMany(genre => genre.Books)
                    .HasForeignKey(book => book.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Book>()
                .ToTable(table => table.HasCheckConstraint("ValidPublishYear", "PublishYear >= 1500"));
            modelBuilder.Entity<Book>()
                .ToTable(table => table.HasCheckConstraint("ValidQuantityInStock", "QuantityInStock >= 1500"));
        }

        private void ConfigureAuthor(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity => {
                // устанавливаем первичный ключ как Id модели
                entity.HasKey(author => author.Id);

                // настраиваем все поля модели
                entity.Property(author => author.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(author => author.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(book => book.Country)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(book => book.BirthDate)
                    .IsRequired();
            });
        }

        private void ConfigureGenre(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity => {
                // устанавливаем первичный ключ как Id модели
                entity.HasKey(genre => genre.Id);

                // настраиваем все поля модели
                entity.Property(author => author.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(author => author.Description)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}

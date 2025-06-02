using Microsoft.EntityFrameworkCore;
using UnistreamTaskWebAPI.Models.Entities;

namespace UnistreamTaskWebAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            //Database.EnsureDeleted();   // Удаляем бд со старой схемой
            //Database.EnsureCreated();   // Добавляем бд
        }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Id)
                .ValueGeneratedNever(); // Отключаем автогенерацию ID

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.Id)
                .IsUnique(); // Уникальный индекс для ID
        }
    }
}

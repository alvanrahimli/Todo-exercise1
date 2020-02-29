using ex1_ToDo.Models;
using Microsoft.EntityFrameworkCore;

namespace ex1_ToDo.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(u => u.Todos)
                .WithOne(t => t.Author)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Todo>()
                .HasOne(t => t.Author)
                .WithMany(u => u.Todos)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Item>()
                .HasOne(i => i.Todo)
                .WithMany(t => t.Items)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
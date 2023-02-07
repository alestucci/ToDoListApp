using Microsoft.EntityFrameworkCore;
using ToDoListApp;

namespace EntityFrameworkClassLibrary
{
    public class ApplicationDbContext:  DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                //optionsBuilder.UseSqlServer(connectionString);
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=ToDo;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Todo>(entity =>
            //{
            //    entity.ToTable("Todo");
            //    entity.Property(e => e.TaskDescription).IsRequired().HasMaxLength(100);
            //});
            modelBuilder.Entity<Todo>().HasData(
                new Todo
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedTime = DateTime.Now,
                    TaskDescription = "Comprare il pane",
                    IsCompleted = false
                },
                new Todo
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedTime = DateTime.Now,
                    TaskDescription = "Pagare le bollette",
                    IsCompleted = false
                },
                new Todo
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedTime = DateTime.Now,
                    TaskDescription = "Andare in banca",
                    IsCompleted = false
                }
                );
        }

        public DbSet<Todo> Todos { get; set; }

    }
}

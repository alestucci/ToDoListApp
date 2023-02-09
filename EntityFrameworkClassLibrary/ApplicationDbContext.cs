using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                //var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
                //var kvaultUrl = configuration.GetValue<string>("VaultUri");

                string secretUri = Environment.GetEnvironmentVariable("KeyVaultUri");
                var client = new SecretClient(new Uri(secretUri), new DefaultAzureCredential());

                string secret = client.GetSecret("DbConnectionString-at").Value.Value;

                optionsBuilder.UseSqlServer(secret);
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

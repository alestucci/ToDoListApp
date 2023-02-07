using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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
                //optionsBuilder.UseSqlServer("Server=tcp:sqlcommondatabase.database.windows.net,1433;Initial Catalog=Todo-Db-At;Persist Security Info=False;User ID=adminne;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

                //string secretUri = "https://azurekeyvault-ac.vault.azure.net";
                string secretUri = Environment.GetEnvironmentVariable("KeyVaultUri");
                var client = new SecretClient(new Uri(secretUri), new DefaultAzureCredential());

                var secret = client.GetSecret("DbConnectionString-at");

                optionsBuilder.UseSqlServer(secret.Value.Value);
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

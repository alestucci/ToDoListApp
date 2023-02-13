using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EntityFrameworkClassLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace ToDoListApp
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            var kvaulturi = configuration.GetValue<string>("KVaultUri");
            var secretClient = new SecretClient(new Uri(kvaulturi), new DefaultAzureCredential());
            var connectionString = secretClient.GetSecret("DbConnectionString-at").Value.Value;


            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

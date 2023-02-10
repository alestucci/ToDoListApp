using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EntityFrameworkClassLibrary;
using EntityFrameworkClassLibrary.Repository;
using EntityFrameworkClassLibrary.UnitOfWork;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ToDoListApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //string DbConnectionString = Environment.GetEnvironmentVariable("KeyVaultConnectionString");
            //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(DbConnectionString));

            var kvaulturi = Environment.GetEnvironmentVariable("KeyVaultUri");
            var secretClient = new SecretClient(new Uri(kvaulturi), new DefaultAzureCredential());
            var connectionString = secretClient.GetSecret("DbConnectionString-at").Value.Value;

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}

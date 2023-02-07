using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EntityFrameworkClassLibrary;
using EntityFrameworkClassLibrary.Repository;
using EntityFrameworkClassLibrary.UnitOfWork;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ToDoListApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            //string connectionString = "Server=tcp:sqlcommondatabase.database.windows.net,1433;Initial Catalog=Todo-Db-At;Persist Security Info=False;User ID=adminne;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            //string secretUri = Environment.GetEnvironmentVariable("KeyVaultUri");
            string secretUri = Environment.GetEnvironmentVariable("KeyVaultUri");
            var client = new SecretClient(new Uri(secretUri), new DefaultAzureCredential());

            Azure.Response<KeyVaultSecret> secret = client.GetSecret("DbConnectionString-at");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(secret.Value.Value));


        }

    }
}

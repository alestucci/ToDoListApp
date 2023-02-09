using EntityFrameworkClassLibrary;
using EntityFrameworkClassLibrary.Repository;
using EntityFrameworkClassLibrary.UnitOfWork;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ToDoListApp
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //string secretUri = Environment.GetEnvironmentVariable("KeyVaultUri");
            //var client = new SecretClient(new Uri(secretUri), new DefaultAzureCredential());
            //Azure.Response<KeyVaultSecret> secret = client.GetSecret("DbConnectionString-at");
            //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(secret.Value.Value));
            
            builder.Services.AddDbContext<ApplicationDbContext>();


        }

    }
}

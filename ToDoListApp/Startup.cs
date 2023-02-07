using EntityFrameworkClassLibrary;
using EntityFrameworkClassLibrary.Repository;
using EntityFrameworkClassLibrary.UnitOfWork;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;

namespace ToDoListApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=ToDo;Integrated Security=True";
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        }

    }
}

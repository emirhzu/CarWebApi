using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EfCore;

namespace CarWebApi.ContextFactory
{
    // A class implementing IDesignTimeDbContextFactory to create instances of RepositoryContext
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        // Creates and configures a DbContext instance
        public RepositoryContext CreateDbContext(string[] args)
        {
            // Set up the configuration builder to load the appsettings.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current directory
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration
                .Build();

            // Create a DbContextOptionsBuilder for RepositoryContext with SQL Server configuration
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("sqlConnection"), // Get the connection string and configure SQL Server
                prj => prj.MigrationsAssembly("CarWebApi")); // Set the assembly containing the migrations

            // Return a new instance of RepositoryContext with the configured options
            return new RepositoryContext(builder.Options);
        }
    }
}

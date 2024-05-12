using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DbLayer
{
    public class FemidaDbContextFactory : IDesignTimeDbContextFactory<ContaxtDBContext>
    {
        public ContaxtDBContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();

            var option = new DbContextOptionsBuilder<ContaxtDBContext>()
                .UseSqlServer(config.GetConnectionString("SqlClient")).Options;
            return new ContaxtDBContext(option);
        }
    }
}

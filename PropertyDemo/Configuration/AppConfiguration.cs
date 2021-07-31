using Microsoft.Extensions.Configuration;
using PropertyDemo.Service;

namespace PropertyDemo.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly IConfiguration _configuration;

        public string ConnectionString => _configuration.GetConnectionString("DefaultConnection");

        public AppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}

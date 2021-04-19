using GamesLoan.Infrastructure.DBConfiguration;
using Microsoft.Extensions.Configuration;

namespace GamesLoan.Infrastructure.IoC
{
    internal class ResolveConfiguration
    {
        public static IConfiguration GetConnectionSettings(IConfiguration configuration)
        {
            return configuration ?? DatabaseConnection.ConnectionConfiguration;
        }
    }
}

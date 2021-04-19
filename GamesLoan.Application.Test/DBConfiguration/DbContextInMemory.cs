using GamesLoan.Infrastructure.DBConfiguration;
using Microsoft.EntityFrameworkCore;

namespace GamesLoan.Application.Test.DBConfiguration
{
    public static class DbContextInMemory
    {
        public static ApplicationContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: "GamersLoan" + System.Guid.NewGuid()).Options;
            return new ApplicationContext(options);
        }
    }
}

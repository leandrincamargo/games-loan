using System.Data;
using System.Data.SqlClient;

namespace GamesLoan.Domain.Test.DBConfiguration
{
    public class DataOptionFactory
    {
        public string DefaultConnection { get; set; }
        public IDbConnection DatabaseConnection => new SqlConnection(DefaultConnection);
    }
}

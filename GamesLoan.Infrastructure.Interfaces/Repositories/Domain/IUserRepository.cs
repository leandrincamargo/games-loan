using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain.Standard;
using System.Collections.Generic;

namespace GamesLoan.Infrastructure.Interfaces.Repositories.Domain
{
    public interface IUserRepository : IDomainRepository<User>
    {
        User GetUserByName(string name);
        User GetUserByEmail(string email);
        User GetUserWithType(int id);
        User GetUserWithType(string login);
        IEnumerable<User> GetUsersWithType();
    }
}

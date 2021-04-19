using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain;
using GamesLoan.Infrastructure.Repositories.Standard;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GamesLoan.Infrastructure.Repositories
{
    public class UserRepository : DomainRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext dbContext) : base(dbContext) { }

        public User GetUserByName(string name)
        {
            return _dbSet.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        }

        public User GetUserByEmail(string email)
        {
            return _dbSet.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
        }

        public User GetUserWithType(int id)
        {
            return _dbSet.Include(x => x.Type).FirstOrDefault(x => x.Id == id);
        }

        public User GetUserWithType(string email)
        {
            return _dbSet.Include(x => x.Type).FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
        }

        public IEnumerable<User> GetUsersWithType()
        {
            return _dbSet.Include(x => x.Type).ToList();
        }
    }
}

using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain;
using GamesLoan.Infrastructure.Repositories.Standard;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GamesLoan.Infrastructure.Repositories
{
    public class GameRepository : DomainRepository<Game>, IGameRepository
    {
        public GameRepository(ApplicationContext dbContext) : base(dbContext) { }

        public Game GetGameByName(string name)
        {
            return _dbSet.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        }

        public Game GetGameByNameWithLoans(string name)
        {
            return _dbSet.Include(x => x.Loans).FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        }

        public Game GetGameByIdWithLoans(int id)
        {
            return _dbSet.Include(x => x.Loans).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Game> GetGamesWithLoans()
        {
            return _dbSet.Include(x => x.Loans).ToList();
        }
    }
}

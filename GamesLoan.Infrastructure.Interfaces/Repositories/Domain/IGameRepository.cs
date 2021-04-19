using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain.Standard;
using System.Collections.Generic;

namespace GamesLoan.Infrastructure.Interfaces.Repositories.Domain
{
    public interface IGameRepository : IDomainRepository<Game>
    {
        Game GetGameByName(string name);
        Game GetGameByNameWithLoans(string name);
        Game GetGameByIdWithLoans(int id);
        IEnumerable<Game> GetGamesWithLoans();
    }
}

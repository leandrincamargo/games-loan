using GamesLoan.Application.Interfaces.Services.Standard;
using GamesLoan.Domain.Entities;
using System.Collections.Generic;

namespace GamesLoan.Application.Interfaces.Services
{
    public interface IGameService : IServiceBase<Game>
    {
        Game CreateGame(int userId, string name);
        Game GetGame(int id);
        Game GetGame(string name);
        IEnumerable<Game> GetGames();
        Game UpdateGame(int id, string name, bool? isActive);
        void DeleteGame(int id);
    }
}

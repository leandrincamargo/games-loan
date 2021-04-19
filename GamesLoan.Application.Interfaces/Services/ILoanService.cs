using GamesLoan.Application.Interfaces.Services.Standard;
using GamesLoan.Domain.Entities;
using System.Collections.Generic;

namespace GamesLoan.Application.Interfaces.Services
{
    public interface ILoanService : IServiceBase<Loan>
    {
        IEnumerable<Loan> GetLoansOfUser(int friendId);
        IEnumerable<Loan> CreateLoans(int friendId, List<string> gameNames);
        IEnumerable<Loan> ReturnGames(int friendId, List<int> ids);
        IEnumerable<Loan> CreateLoansWithFriendEmail(string friendEmail, List<int> gameIds);
        IEnumerable<Loan> ReturnGamesWithFriendEmail(string friendEmail, List<int> ids);
    }
}

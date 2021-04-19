using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain.Standard;
using System.Collections.Generic;

namespace GamesLoan.Infrastructure.Interfaces.Repositories.Domain
{
    public interface ILoanRepository : IDomainRepository<Loan>
    {
        Loan GetLoanByUserId(int userId, int gameId);
        IEnumerable<Loan> GetLoansByUserId(int userId);
        Loan GetLoanById(int id);
        IEnumerable<Loan> GetLoansById(List<int> ids);
    }
}

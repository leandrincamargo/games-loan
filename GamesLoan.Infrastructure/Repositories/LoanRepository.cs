using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain;
using GamesLoan.Infrastructure.Repositories.Standard;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GamesLoan.Infrastructure.Repositories
{
    public class LoanRepository : DomainRepository<Loan>, ILoanRepository
    {
        public LoanRepository(ApplicationContext dbContext) : base(dbContext) { }

        public Loan GetLoanByUserId(int userId, int gameId)
        {
            return _dbSet.Include(x => x.Game).FirstOrDefault(x => x.Friend.Id == userId && x.Game.Id == gameId);
        }

        public IEnumerable<Loan> GetLoansByUserId(int userId)
        {
            return _dbSet.Include(x => x.Game).Where(x => x.Friend.Id == userId).ToList();
        }

        public Loan GetLoanById(int id)
        {
            return _dbSet.Include(x => x.Friend).Include(x => x.Game).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Loan> GetLoansById(List<int> ids)
        {
            return _dbSet.Include(x => x.Friend).Include(x => x.Game).Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}

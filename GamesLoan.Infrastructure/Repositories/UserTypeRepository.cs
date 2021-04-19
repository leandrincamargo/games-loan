using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain;
using GamesLoan.Infrastructure.Repositories.Standard;

namespace GamesLoan.Infrastructure.Repositories
{
    public class UserTypeRepository : DomainRepository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(ApplicationContext dbContext) : base(dbContext) { }
    }
}

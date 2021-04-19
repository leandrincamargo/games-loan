using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain.Standard;

namespace GamesLoan.Infrastructure.Repositories.Standard
{
    public class DomainRepository<TEntity> : Repository<TEntity>, IDomainRepository<TEntity> where TEntity : class
    {
        protected DomainRepository(ApplicationContext dbContext) : base(dbContext) { }
    }
}

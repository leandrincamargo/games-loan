using GamesLoan.Infrastructure.Interfaces.Repositories.Standard;

namespace GamesLoan.Infrastructure.Interfaces.Repositories.Domain.Standard
{
    public interface IDomainRepository<TEntity> : IRepository<TEntity> where TEntity : class { }
}

using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Application.Services.Standard;
using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain;

namespace GamesLoan.Application.Services
{
    public class UserTypeService : ServiceBase<UserType>, IUserTypeService
    {
        public UserTypeService(IUserTypeRepository repository) : base(repository) { }
    }
}

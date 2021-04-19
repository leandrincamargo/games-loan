using GamesLoan.Application.Interfaces.Services.Standard;
using GamesLoan.Domain.Entities;
using System.Collections.Generic;

namespace GamesLoan.Application.Interfaces.Services
{
    public interface IUserService : IServiceBase<User>
    {
        User CreateUser(string name, string email, string password, string phoneNumber, int userTypeId);
        User GetUser(int id);
        User GetUser(string email, string password);
        IEnumerable<User> GetUsers();
        User UpdateUser(int id, string name, string email, string password, string phoneNumber, int? userTypeId);
        void DeleteUser(int id);
    }
}

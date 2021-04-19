using GamesLoan.Domain.Entities;

namespace GamesLoan.Application.Interfaces.Services
{
    public interface IAutenticationService
    {
        string HashPassword(string password);
        bool IsPasswordValid(string password, string hashedPassword);
        string GenerateToken(User user, string audience);
    }
}

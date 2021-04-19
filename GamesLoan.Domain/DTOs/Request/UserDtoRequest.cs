using GamesLoan.Domain.Enums;

namespace GamesLoan.Domain.DTOs.Request
{
    public class UserDtoRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public UserTypeEnum? UserType { get; set; }
    }
}

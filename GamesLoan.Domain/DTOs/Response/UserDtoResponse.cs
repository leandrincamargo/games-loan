using GamesLoan.Domain.Entities;

namespace GamesLoan.Domain.DTOs.Response
{
    public class UserDtoResponse
    {
        public UserDtoResponse(User user)
        {
            Id = user.Id;
            Name = user.Name;
            UserType = user.Type.Name;
            UserTypeId = user.Type.Id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
    }
}

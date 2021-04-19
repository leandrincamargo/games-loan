namespace GamesLoan.Domain.DTOs.Response
{
    public class LoggedUserDto
    {
        public LoggedUserDto(string userName, string token)
        {
            UserName = userName;
            Token = token;
        }

        public string UserName { get; set; }
        public string Token { get; set; }
    }
}

using GamesLoan.Domain.Entities;

namespace GamesLoan.Domain.DTOs.Response
{
    public class LoanDtoResponse
    {
        public LoanDtoResponse(Loan loan)
        {
            Id = loan.Id;
            GameId = loan.Game.Id;
            GameName = loan.Game.Name;
        }

        public int Id { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
    }
}

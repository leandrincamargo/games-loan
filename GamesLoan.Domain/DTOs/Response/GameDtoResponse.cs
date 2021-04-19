using GamesLoan.Domain.Entities;
using System.Linq;

namespace GamesLoan.Domain.DTOs.Response
{
    public class GameDtoResponse
    {
        public GameDtoResponse(Game game)
        {
            var loanedUser = game.Loans.FirstOrDefault(x => !x.DevolutionDate.HasValue);
            Id = game.Id;
            Name = game.Name;
            Available = loanedUser == null;
            LoanedUserId = loanedUser?.Friend.Id;
            LoanedUserName = loanedUser?.Friend.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }
        public int? LoanedUserId { get; set; }
        public string LoanedUserName { get; set; }

    }
}

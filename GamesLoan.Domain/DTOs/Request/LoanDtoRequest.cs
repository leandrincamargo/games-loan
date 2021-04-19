using System.Collections.Generic;

namespace GamesLoan.Domain.DTOs.Request
{
    public class LoanDtoRequest
    {
        public string UserEmail { get; set; }
        public List<int> Ids { get; set; }
    }
}

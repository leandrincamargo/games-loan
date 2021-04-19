using System;

namespace GamesLoan.Domain.Entities
{
    public class Loan: IIdentityEntity
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public Friend Friend { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? DevolutionDate { get; set; }

    }
}

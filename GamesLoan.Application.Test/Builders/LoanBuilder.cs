using GamesLoan.Domain.Entities;
using System;

namespace GamesLoan.Application.Test.Builders
{
    public class LoanBuilder
    {
        private Loan loan;
        public Loan CreateLoan()
        {
            loan = new Loan()
            {
                Friend = new Friend(),
                Game = new Game(),
                LoanDate = DateTime.Now.AddDays(-4)
            };
            return loan;
        }
    }
}

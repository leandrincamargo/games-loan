using System.Collections.Generic;

namespace GamesLoan.Domain.Entities
{
    public class Friend: User
    {
        public Friend()
        {
            this.Loans = new HashSet<Loan>();
        }

        public virtual ICollection<Loan> Loans { get; set; }
    }
}

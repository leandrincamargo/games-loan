using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesLoan.Domain.Entities
{
    public class Game : IIdentityEntity
    {
        public Game()
        {
            this.Loans = new HashSet<Loan>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Loan> Loans { get; set; }
    }
}

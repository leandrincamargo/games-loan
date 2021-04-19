using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesLoan.Domain.Entities
{
    public class User : IIdentityEntity
    {
        public User()
        {
            this.Games = new HashSet<Game>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public UserType Type { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}

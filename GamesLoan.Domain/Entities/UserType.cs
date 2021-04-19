using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesLoan.Domain.Entities
{
    public class UserType : IIdentityEntity
    {
        public UserType()
        {
            this.Users = new HashSet<User>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}

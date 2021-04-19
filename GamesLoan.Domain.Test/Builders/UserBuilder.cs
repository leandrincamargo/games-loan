using GamesLoan.Domain.Entities;
using System;

namespace GamesLoan.Domain.Test.Builders
{
    public class UserBuilder
    {
        private User user;
        public User CreateUser()
        {
            user = new User()
            {
                Name = "Test User 123",
                Email = "email@test.com",
                Password = "password123",
                PhoneNumber = "159487",
                Type = new UserType(),
                CreationDate = DateTime.Now,
            };
            return user;
        }
    }
}

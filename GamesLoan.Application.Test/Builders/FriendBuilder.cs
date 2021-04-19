using GamesLoan.Domain.Entities;
using System;

namespace GamesLoan.Application.Test.Builders
{
    public class FriendBuilder
    {
        private Friend friend;
        public Friend CreateFriend()
        {
            friend = new Friend()
            {
                Name = "Test User 123",
                Email = "email@test.com",
                Password = "password123",
                PhoneNumber = "(99) 99999-9999",
                Type = new UserType(),
                CreationDate = DateTime.Now,
            };
            return friend;
        }
    }
}

using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Application.Services;
using GamesLoan.Application.Test.Builders;
using GamesLoan.Application.Test.DBConfiguration;
using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.Repositories;
using System;
using System.Linq;
using Xunit;

namespace GamesLoan.Application.Test.Services
{
    public class UserServiceTest : IDisposable
    {
        private ApplicationContext dbContext;
        private IUserService userService;
        private UserBuilder builder;

        private UserType typeMock;

        public void Dispose()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Fact]
        public void CreateUserTest()
        {
            Build();

            var userMock = builder.CreateUser();

            var result = userService.CreateUser(userMock.Name, userMock.Email, userMock.Password, userMock.PhoneNumber, typeMock.Id);

            Assert.True(result.Id > 0);
            Assert.Equal(userMock.Name, result.Name); ;
            Assert.Equal(userMock.Email, result.Email);
            Assert.Equal(userMock.PhoneNumber, result.PhoneNumber);
        }

        [Fact]
        public void GetUserTest()
        {
            Build();

            var userMock = builder.CreateUser();

            var user = userService.CreateUser(userMock.Name, userMock.Email, userMock.Password, userMock.PhoneNumber, typeMock.Id);
            var result = userService.GetUser(user.Email, userMock.Password);

            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public void GetUsersTest()
        {
            Build();

            var user = userService.Add(builder.CreateUser());
            var result = userService.GetUsers();

            Assert.True(result.Count() > 0);
            Assert.Contains(result, x => x.Id == user.Id);
        }

        [Theory]
        [InlineData("User Test 65498")]
        public void UpdateUserTest(string name)
        {
            Build();

            var user = userService.Add(builder.CreateUser());
            var result = userService.UpdateUser(user.Id, name, null, null, null, null);

            Assert.Equal(name, result.Name);
        }

        [Fact]
        public void DeleteUserTest()
        {
            Build();

            var inserted = userService.Add(builder.CreateUser());
            userService.DeleteUser(inserted.Id);

            Assert.True(true);
        }

        private void Build()
        {
            dbContext = DbContextInMemory.GetContext();
            var userTypeService = new UserTypeService(new UserTypeRepository(dbContext));


            userService = new UserService(new UserRepository(dbContext), new AutenticationService(), userTypeService);
            builder = new UserBuilder();

            MockUserType(userTypeService);
        }

        private void MockUserType(UserTypeService userTypeService)
        {
            typeMock = userTypeService.Add(new UserType { Name = "TEST", Description = "Mock Test" });
        }
    }
}

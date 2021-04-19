using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain;
using GamesLoan.Infrastructure.Repositories;
using GamesLoan.Infrastructure.Test.Builders;
using GamesLoan.Infrastructure.Test.DBConfiguration;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using Xunit;

namespace GamesLoan.Domain.Test.Services
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly ApplicationContext dbContext;
        private readonly IDbContextTransaction transaction;

        private readonly IUserRepository userRepository;
        private readonly UserBuilder builder;

        public UserRepositoryTest()
        {
            dbContext = new EntityFrameworkConnection().DataBaseConfiguration();
            userRepository = new UserRepository(dbContext);
            builder = new UserBuilder();
            transaction = dbContext.Database.BeginTransaction();
        }

        public void Dispose()
        {
            transaction.Rollback();
        }

        [Fact]
        public void GetAll()
        {
            var game = userRepository.Add(builder.CreateUser());
            var result = userRepository.GetAll();
            Assert.Equal(result.OrderBy(u => u.Id).LastOrDefault().Id, game.Id);
        }

        [Fact]
        public void GetById()
        {
            var game = userRepository.Add(builder.CreateUser());
            var result = userRepository.GetById(game.Id);
            Assert.Equal(result.Id, game.Id);
        }

        [Fact]
        public void Add()
        {
            var result = userRepository.Add(builder.CreateUser());
            Assert.True(result.Id > 0);
        }

        [Theory]
        [InlineData("User Test 65498")]
        public void Update(string name)
        {
            var inserted = userRepository.Add(builder.CreateUser());
            inserted.Name = name;

            var result = userRepository.Update(inserted);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Remove()
        {
            var inserted = userRepository.Add(builder.CreateUser());
            var result = userRepository.Remove(inserted.Id);
            Assert.True(result);
        }

        [Fact]
        public void RemoveObj()
        {
            var inserted = userRepository.Add(builder.CreateUser());
            var result = userRepository.Remove(inserted);
            Assert.Equal(1, result);
        }
    }
}

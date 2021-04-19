using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Application.Services;
using GamesLoan.Domain.Test.Builders;
using GamesLoan.Domain.Test.DBConfiguration;
using GamesLoan.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using Xunit;

namespace GamesLoan.Domain.Test.Services
{
    public class GameServiceTest : IDisposable
    {
        private readonly IDbContextTransaction transaction;

        private readonly IGameService gameService;
        private readonly GameBuilder builder;

        public GameServiceTest()
        {
            var dbContext = new EntityFrameworkConnection().DataBaseConfiguration();
            var gameRepository = new GameRepository(dbContext);

            gameService = new GameService(gameRepository);
            builder = new GameBuilder();
            transaction = dbContext.Database.BeginTransaction();
        }

        public void Dispose()
        {
            transaction.Rollback();
        }

        [Fact]
        public void GetAll()
        {
            var game = gameService.Add(builder.CreateGame());
            var result = gameService.GetAll();
            Assert.Equal(result.OrderBy(u => u.Id).LastOrDefault().Id, game.Id);
        }

        [Fact]
        public void GetById()
        {
            var game = gameService.Add(builder.CreateGame());
            var result = gameService.GetById(game.Id);
            Assert.Equal(result.Id, game.Id);
        }

        [Fact]
        public void Add()
        {
            var gameMock = builder.CreateGame();

            var result = gameService.Add(gameMock);

            Assert.True(result.Id > 0);
            Assert.Equal(gameMock.Name, result.Name); ;
            Assert.Equal(gameMock.IsActive, result.IsActive);
            Assert.Equal(gameMock.CreationDate, result.CreationDate);
        }

        [Theory]
        [InlineData("Game Test 65498")]
        public void Update(string name)
        {
            var inserted = gameService.Add(builder.CreateGame());
            inserted.Name = name;

            var result = gameService.Update(inserted);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Remove()
        {
            var inserted = gameService.Add(builder.CreateGame());
            var result = gameService.Remove(inserted.Id);
            Assert.True(result);
        }

        [Fact]
        public void RemoveObj()
        {
            var inserted = gameService.Add(builder.CreateGame());
            var result = gameService.Remove(inserted);
            Assert.Equal(1, result);
        }
    }
}

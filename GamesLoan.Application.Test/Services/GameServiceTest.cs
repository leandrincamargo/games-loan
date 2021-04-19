using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Application.Services;
using GamesLoan.Application.Test.Builders;
using GamesLoan.Application.Test.DBConfiguration;
using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.Repositories;
using System;
using System.Linq;
using Xunit;

namespace GamesLoan.Application.Test.Services
{
    public class GameServiceTest : IDisposable
    {
        private ApplicationContext dbContext;
        private IGameService gameService;
        private GameBuilder builder;

        public void Dispose()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Fact]
        public void CreateGameTest()
        {
            Build();

            var gameMock = builder.CreateGame();

            var result = gameService.CreateGame(gameMock.Name);

            Assert.True(result.Id > 0);
            Assert.True(result.IsActive);
            Assert.Equal(gameMock.Name, result.Name); ;
        }

        [Fact]
        public void GetGameByNameTest()
        {
            Build();

            var gameMock = builder.CreateGame();

            var game = gameService.Add(gameMock);
            var result = gameService.GetGame(gameMock.Name);

            Assert.Equal(result.Id, game.Id);
        }

        [Fact]
        public void GetGamesTest()
        {
            Build();

            var gameMock = builder.CreateGame();
            var game = gameService.Add(gameMock);
            var result = gameService.GetGames();

            Assert.True(result.Count() > 0);
            Assert.Contains(result, x => x.Name == gameMock.Name);
        }

        [Theory]
        [InlineData("Game Test 65498")]
        public void UpdateGameTest(string name)
        {
            Build();

            var inserted = gameService.Add(builder.CreateGame());

            var result = gameService.UpdateGame(inserted.Id, name, null);

            Assert.Equal(name, result.Name);
        }

        [Fact]
        public void DeleteGameTest()
        {
            Build();

            var inserted = gameService.Add(builder.CreateGame());

            gameService.DeleteGame(inserted.Id);

            Assert.True(true);
        }

        private void Build()
        {
            dbContext = DbContextInMemory.GetContext();

            gameService = new GameService(new GameRepository(dbContext));
            builder = new GameBuilder();
        }
    }
}

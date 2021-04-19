using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Application.Services;
using GamesLoan.Application.Test.Builders;
using GamesLoan.Application.Test.DBConfiguration;
using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GamesLoan.Application.Test.Services
{
    public class LoanServiceTest : IDisposable
    {
        private ApplicationContext dbContext;
        private ILoanService loanService;

        private User friendMock;
        private Game gameMock;

        public void Dispose()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Fact]
        public void GetLoansOfUserTest()
        {
            Build();

            var loans = loanService.CreateLoans(friendMock.Id, new List<string> { gameMock.Name });
            var result = loanService.GetLoansOfUser(friendMock.Id);

            Assert.Equal(loans.Count(), result.Count());
        }

        [Fact]
        public void CreateLoansTest()
        {
            Build();

            var result = loanService.CreateLoans(friendMock.Id, new List<string> { gameMock.Name });

            Assert.True(result.Any());
        }

        [Fact]
        public void ReturnGamesTest()
        {
            Build();

            var loans = loanService.CreateLoans(friendMock.Id, new List<string> { gameMock.Name });
            var result = loanService.ReturnGames(friendMock.Id, loans.Select(x => x.Id).ToList());

            Assert.True(result.All(x => x.DevolutionDate.HasValue));
        }

        [Fact]
        public void CreateLoansWithFriendEmailTest()
        {
            Build();

            var result = loanService.CreateLoansWithFriendEmail(friendMock.Email, new List<int> { gameMock.Id });

            Assert.True(result.Any());
        }

        [Fact]
        public void ReturnGamesWithFriendEmailTest()
        {
            Build();

            var loans = loanService.CreateLoans(friendMock.Id, new List<string> { gameMock.Name });
            var result = loanService.ReturnGamesWithFriendEmail(friendMock.Email, loans.Select(x => x.Id).ToList());

            Assert.True(result.All(x => x.DevolutionDate.HasValue));
        }

        private void Build()
        {
            dbContext = DbContextInMemory.GetContext();
            var userRepository = new UserRepository(dbContext);
            var gameRepository = new GameRepository(dbContext);
            loanService = new LoanService(new LoanRepository(dbContext), userRepository, gameRepository);

            MockUser(userRepository);
            MockGame(gameRepository);
        }

        private void MockUser(UserRepository userRepository)
        {
            friendMock = userRepository.Add(new FriendBuilder().CreateFriend());
        }

        private void MockGame(GameRepository gameRepository)
        {
            gameMock = gameRepository.Add(new GameBuilder().CreateGame());
        }
    }
}

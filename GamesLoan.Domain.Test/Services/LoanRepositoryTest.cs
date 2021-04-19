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
    public class LoanRepositoryTest : IDisposable
    {
        private readonly ApplicationContext dbContext;
        private readonly IDbContextTransaction transaction;

        private readonly ILoanRepository loanRepository;
        private readonly LoanBuilder builder;

        public LoanRepositoryTest()
        {
            dbContext = new EntityFrameworkConnection().DataBaseConfiguration();
            loanRepository = new LoanRepository(dbContext);
            builder = new LoanBuilder();
            transaction = dbContext.Database.BeginTransaction();
        }

        public void Dispose()
        {
            transaction.Rollback();
        }

        [Fact]
        public void GetAll()
        {
            var game = loanRepository.Add(builder.CreateLoan());
            var result = loanRepository.GetAll();
            Assert.Equal(result.OrderBy(u => u.Id).LastOrDefault().Id, game.Id);
        }

        [Fact]
        public void GetById()
        {
            var game = loanRepository.Add(builder.CreateLoan());
            var result = loanRepository.GetById(game.Id);
            Assert.Equal(result.Id, game.Id);
        }

        [Fact]
        public void Add()
        {
            var result = loanRepository.Add(builder.CreateLoan());
            Assert.True(result.Id > 0);
        }

        [Theory]
        [InlineData("2021-05-05")]
        public void Update(string devolutionDate)
        {
            var inserted = loanRepository.Add(builder.CreateLoan());
            inserted.DevolutionDate = Convert.ToDateTime(devolutionDate);

            var result = loanRepository.Update(inserted);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Remove()
        {
            var inserted = loanRepository.Add(builder.CreateLoan());
            var result = loanRepository.Remove(inserted.Id);
            Assert.True(result);
        }

        [Fact]
        public void RemoveObj()
        {
            var inserted = loanRepository.Add(builder.CreateLoan());
            var result = loanRepository.Remove(inserted);
            Assert.Equal(1, result);
        }
    }
}

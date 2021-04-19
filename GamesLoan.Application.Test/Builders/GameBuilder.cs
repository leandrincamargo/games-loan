using GamesLoan.Domain.Entities;
using System;

namespace GamesLoan.Application.Test.Builders
{
    public class GameBuilder
    {
        private Game game;
        public Game CreateGame()
        {
            game = new Game()
            {
                Name = "Test Game 123",
                IsActive = true,
                CreationDate = DateTime.Now.AddDays(-90),
            };
            return game;
        }
    }
}

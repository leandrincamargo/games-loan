using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Application.Services.Standard;
using GamesLoan.Domain;
using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain;
using System;
using System.Collections.Generic;

namespace GamesLoan.Application.Services
{
    public class GameService : ServiceBase<Game>, IGameService
    {
        private readonly IGameRepository _repository;
        public GameService(IGameRepository repository) : base(repository) { _repository = repository; }

        public Game CreateGame(string name)
        {
            try
            {
                ValidateNew(name);

                var game = MountNewGame(name);
                var newGame = base.Add(game);
                return newGame;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Game GetGame(int id)
        {
            try
            {
                var game = _repository.GetGameByIdWithLoans(id);
                if (game == null)
                    throw new ValidationException("Game not found");

                return game;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Game GetGame(string name)
        {
            try
            {
                var game = _repository.GetGameByName(name);
                if (game == null)
                    throw new ValidationException("Game not found");

                return game;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Game> GetGames()
        {
            try
            {
                var games = _repository.GetGamesWithLoans();
                return games;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Game UpdateGame(int id, string name, bool? isActive)
        {
            try
            {
                ValidateUpdate(id, name);

                var game = _repository.GetById(id);
                if (game == null)
                    throw new ValidationException($"Game Not Found '{name}'");

                UpdateValues(ref game, name, isActive);

                base.Update(game);

                return game;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteGame(int gameId)
        {
            try
            {
                var deletedGame = base.Remove(gameId);
                if (deletedGame == false)
                    throw new ValidationException("Game Not Found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidateNew(string name)
        {
            var existingGame = _repository.GetGameByName(name);
            if (existingGame != null)
                throw new ValidationException($"There's already a game with this name. 'Name: {name}'");
        }

        private void ValidateUpdate(int id, string name)
        {
            var game = base.GetById(id);
            if (game == null)
                throw new ValidationException($"Game not found '{id}'");

            ValidateNew(name);
        }

        private static Game MountNewGame(string name)
        {
            var newGame = new Game();
            newGame.CreationDate = DateTime.Now;

            UpdateValues(ref newGame, name, true);

            return newGame;
        }

        private static void UpdateValues(ref Game game, string name, bool? isActive)
        {
            if (!string.IsNullOrWhiteSpace(name))
                game.Name = name;
            if (isActive.HasValue)
                game.IsActive = isActive.Value;
        }
    }
}

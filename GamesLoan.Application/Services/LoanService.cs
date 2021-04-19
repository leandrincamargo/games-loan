using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Application.Services.Standard;
using GamesLoan.Domain.Entities;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GamesLoan.Application.Services
{
    public class LoanService : ServiceBase<Loan>, ILoanService
    {
        private readonly ILoanRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;

        public LoanService(ILoanRepository repository, IUserRepository userRepository, IGameRepository gameRepository) : base(repository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
        }

        public IEnumerable<Loan> GetLoansOfUser(int friendId)
        {
            try
            {
                var loans = _repository.GetLoansByUserId(friendId);
                if (loans == null || !loans.Any())
                    throw new ValidationException();

                return loans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Loan> CreateLoans(int friendId, List<string> gameNames)
        {
            try
            {
                var newLoans = new List<Loan>();
                var user = GetFriend(friendId);

                foreach (var gameName in gameNames)
                {
                    var loan = CreateLoanByGameName((Friend)user, gameName);
                    newLoans.Add(loan);
                }

                return newLoans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Loan> ReturnGames(int friendId, List<int> ids)
        {
            try
            {
                var loans = _repository.GetLoansById(ids);
                foreach (var loan in loans)
                {
                    if (loan.Friend.Id != friendId)
                        throw new ValidationException($"Only games that you loaned can be returned: {loan.Id}");
                    if (loan.DevolutionDate.HasValue)
                        throw new ValidationException($"The game that you loaned already returned: {loan.Id}");

                    loan.DevolutionDate = DateTime.Now;
                    base.Update(loan);
                }

                return loans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Loan> CreateLoansWithFriendEmail(string friendEmail, List<int> gameIds)
        {
            try
            {
                var newLoans = new List<Loan>();
                var user = GetFriendByEmail(friendEmail);

                foreach (var gameId in gameIds)
                {
                    var loan = CreateLoan((Friend)user, gameId);
                    newLoans.Add(loan);
                }

                return newLoans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Loan> ReturnGamesWithFriendEmail(string friendEmail, List<int> ids)
        {
            try
            {
                var loans = _repository.GetLoansById(ids);
                foreach (var loan in loans)
                {
                    if (loan.Friend.Email.ToLower() != friendEmail.ToLower())
                        throw new ValidationException($"This game was not loaned by this email: --Id: {loan.Id} --Email: {friendEmail}");
                    if (loan.DevolutionDate.HasValue)
                        throw new ValidationException($"The game that you loaned already returned: {loan.Id}");

                    loan.DevolutionDate = DateTime.Now;
                    base.Update(loan);
                }

                return loans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Loan CreateLoanByGameName(Friend friend, string gameName)
        {
            try
            {
                var game = GetGameByName(gameName);

                var loan = MountNewLoan(friend, game);
                var newLoan = base.Add(loan);

                return newLoan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Loan CreateLoan(Friend friend, int gameId)
        {
            try
            {
                var game = GetGameById(gameId);

                var loan = MountNewLoan(friend, game);
                var newLoan = base.Add(loan);

                return newLoan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private User GetFriend(int friendId)
        {
            var user = _userRepository.GetUserWithType(friendId);
            if (user == null)
                throw new ValidationException("User not found");
            return user;
        }

        private User GetFriendByEmail(string friendEmail)
        {
            var user = _userRepository.GetUserByEmail(friendEmail);
            if (user == null)
                throw new ValidationException("Friend not found");
            return user;
        }

        private Game GetGameById(int id)
        {
            var game = _gameRepository.GetGameByIdWithLoans(id);
            if (game == null)
                throw new ValidationException("Game not found");
            if (game.Loans.Any(x => x.DevolutionDate == null))
                throw new ValidationException("Game not available");
            return game;
        }

        private Game GetGameByName(string name)
        {
            var game = _gameRepository.GetGameByNameWithLoans(name);
            if (game == null)
                throw new ValidationException("Game not found");
            if (game.Loans.Any(x => x.DevolutionDate == null))
                throw new ValidationException("Game not available");
            return game;
        }

        private static Loan MountNewLoan(Friend friend, Game game)
        {
            var newLoan = new Loan();
            newLoan.LoanDate = DateTime.Now;
            newLoan.Friend = friend;
            newLoan.Game = game;

            return newLoan;
        }
    }
}

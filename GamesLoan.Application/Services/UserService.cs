using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Application.Services.Standard;
using GamesLoan.Domain;
using GamesLoan.Domain.Entities;
using GamesLoan.Domain.Enums;
using GamesLoan.Infrastructure.Interfaces.Repositories.Domain;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GamesLoan.Application.Services
{
    public class UserService : ServiceBase<User>, IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IAutenticationService _autenticationService;
        private readonly IUserTypeService _userTypeService;

        public UserService(IUserRepository repository, IAutenticationService autenticationService, IUserTypeService userTypeService) : base(repository)
        {
            _repository = repository;
            _autenticationService = autenticationService;
            _userTypeService = userTypeService;
        }

        public User CreateUser(string name, string email, string password, string phoneNumber, int userTypeId)
        {
            try
            {
                ValidateNew(name, email, password, phoneNumber, userTypeId);

                var type = _userTypeService.GetById(userTypeId);

                var hashPassword = !string.IsNullOrWhiteSpace(password) ? _autenticationService.HashPassword(password) : "";

                var user = MountNewUser(name, email, phoneNumber, type, hashPassword);
                if (type.Id == (int)UserTypeEnum.FRIEND)
                    user = MountNewFriend(name, email, phoneNumber, type, hashPassword);

                var newUser = base.Add(user);

                return newUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetUser(int id)
        {
            try
            {
                var user = _repository.GetUserWithType(id);
                if (user == null)
                    throw new ValidationException("User not found");

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetUser(string email, string password)
        {
            try
            {
                var user = _repository.GetUserWithType(email);
                if (user == null)
                    throw new ValidationException("User not found");
                if (!_autenticationService.IsPasswordValid(password, user.Password))
                    throw new ValidationException("Incorrect password");

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            try
            {
                var users = _repository.GetUsersWithType();
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User UpdateUser(int id, string name, string email, string password, string phoneNumber, int? userTypeId)
        {
            try
            {
                ValidateUpdate(id, name, email, phoneNumber, userTypeId);

                var user = _repository.GetUserWithType(id);
                var userType = _userTypeService.GetById(userTypeId);

                var hashPassword = !string.IsNullOrWhiteSpace(password) ? _autenticationService.HashPassword(password) : "";

                UpdateValues(ref user, name, email, phoneNumber, userType, hashPassword);

                base.Update(user);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                var deletedUser = base.Remove(id);
                if (!deletedUser)
                    throw new ValidationException("User not found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidateNew(string name, string email, string password, string phoneNumber, int userTypeId)
        {
            var userType = _userTypeService.GetById(userTypeId);
            if (userType == null)
                throw new ValidationException($"User type not found. 'UserTypeId: {userTypeId}'");

            var existingUser = _repository.GetUserByName(name);
            if (existingUser != null)
                throw new ValidationException($"There's already a user with this name. 'Name: {name}'");
            existingUser = _repository.GetUserByEmail(email);
            if (existingUser != null)
                throw new ValidationException($"There's already a user with this email. 'Email: {email}'");
            if (userTypeId == (int)UserTypeEnum.ADMIN && string.IsNullOrWhiteSpace(password))
                throw new ValidationException($"Password is invalid, please try another. 'Password: {password}'");
            if (!Regex.Match(phoneNumber, @"^\([1-9]{2}\) (?:[2-8]|9[1-9])[0-9]{3}\-[0-9]{4}$").Success)
                throw new ValidationException($"Phone number is invalid, please try like this '(99) 99999-9999' . 'Phone: {phoneNumber}'");
        }

        private void ValidateUpdate(int userId, string name, string email, string phoneNumber, int? userTypeId)
        {
            var user = base.GetById(userId);
            if (user == null)
                throw new ValidationException($"User not found '{userId}'");

            if (userTypeId.HasValue)
            {
                var userType = _userTypeService.GetById(userTypeId.Value);
                if (userType == null)
                    throw new ValidationException($"User type not found. 'UserTypeId: {userTypeId}'");
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var existingUser = _repository.GetUserByName(name);
                if (existingUser != null)
                    throw new ValidationException($"There's already a user with this name. 'Name: {name}'");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var existingUser = _repository.GetUserByEmail(email);
                if (existingUser != null)
                    throw new ValidationException($"There's already a user with this email. 'Email: {email}'");
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber) && !Regex.Match(phoneNumber, @"^\([1-9]{2}\) (?:[2-8]|9[1-9])[0-9]{3}\-[0-9]{4}$").Success)
                throw new ValidationException($"Phone number is invalid, please try like this '(99) 99999-9999' . 'Phone: {phoneNumber}'");
        }

        private static User MountNewUser(string name, string email, string phoneNumber, UserType userType, string passwordHash)
        {
            var newUser = new User();
            UpdateValues(ref newUser, name, email, phoneNumber, userType, passwordHash);
            newUser.CreationDate = DateTime.Now;

            return newUser;
        }

        private static void UpdateValues(ref User user, string name, string email, string phoneNumber, UserType userType, string passwordHash)
        {
            if (userType != null)
                user.Type = userType;
            if (!string.IsNullOrWhiteSpace(name))
                user.Name = name;
            if (!string.IsNullOrWhiteSpace(email))
                user.Email = email;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
                user.PhoneNumber = phoneNumber;
            if (!string.IsNullOrWhiteSpace(passwordHash))
                user.Password = passwordHash;
        }

        private static Friend MountNewFriend(string name, string email, string phoneNumber, UserType userType, string passwordHash)
        {
            var newFriend = new Friend();
            newFriend.CreationDate = DateTime.Now;
            newFriend.Type = userType;
            newFriend.Name = name;
            newFriend.Email = email;
            newFriend.PhoneNumber = phoneNumber;
            newFriend.Password = passwordHash;

            return newFriend;
        }
    }
}

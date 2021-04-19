using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Domain;
using GamesLoan.Domain.DTOs.Request;
using GamesLoan.Domain.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesLoan.Api.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IAutenticationService _autenticationService;

        public UserController(IUserService userService, IAutenticationService autenticationService)
        {
            _userService = userService;
            _autenticationService = autenticationService;
        }

        /// <summary>
        /// This route is used to return an user by id
        /// </summary>
        /// <param name="id">Id of The User</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDtoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult GetById([FromRoute(Name = "id")] int id)
        {
            try
            {
                if (GetUserId() != id && GetUserRole() != Constants.Administrator)
                    return Forbid();

                var user = _userService.GetUser(id);

                return Ok(new UserDtoResponse(user));
            }
            catch (ValidationException e)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This route is used to return all users
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDtoResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Get()
        {
            try
            {
                var users = _userService.GetUsers();
                return Ok(users.Select(x => new UserDtoResponse(x)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This route is used to user login
        /// </summary>
        /// <param name="audience">Audience to validate the app</param>
        /// <param name="request">
        /// Email = Email of the User
        /// <br /> 
        /// Password = Password of the User
        /// </param>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoggedUserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(LoggedUserDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public IActionResult Login([FromHeader] string audience, [FromBody] LoginUserDto request)
        {
            try
            {
                var user = _userService.GetUser(request.Email, request.Password);
                var token = _autenticationService.GenerateToken(user, audience);

                return Ok(new LoggedUserDto(user.Name, token));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This route is used to create new users
        /// </summary>
        /// <param name="request">
        /// Name = Name of the User
        /// <br /> 
        /// Email = Email of the User
        /// <br /> 
        /// Password = Password of the User
        /// <br /> 
        /// PhoneNumber = PhoneNumber of the User
        /// <br /> 
        /// UserType = Type of User:
        /// <br /> 
        ///    1 - ADMIN
        /// <br /> 
        ///    2 - FRIEND
        /// </param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDtoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public IActionResult Post([FromBody] UserDtoRequest request)
        {
            try
            {
                var user = _userService.CreateUser(request.Name, request.Email, request.Password, request.PhoneNumber, (int)request.UserType);
                return Created($"User: {user.Id}", new UserDtoResponse(user));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This route is used to edit users
        /// </summary>
        /// <param name="id">Id of The User</param>
        /// <param name="request">
        /// Name = Name of the User
        /// <br /> 
        /// Email = Email of the User
        /// <br /> 
        /// Password = Password of the User
        /// <br /> 
        /// PhoneNumber = PhoneNumber of the User
        /// <br /> 
        /// UserType = Type of User:
        /// <br /> 
        ///    1 - ADMIN
        /// <br /> 
        ///    2 - FRIEND
        /// </param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDtoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult Put([FromRoute(Name = "id")] int id, [FromBody] UserDtoRequest request)
        {
            try
            {
                if (GetUserId() != id && GetUserRole() != Constants.Administrator)
                    return Forbid();

                var user = _userService.UpdateUser(id, request.Name, request.Email, request.Password, request.PhoneNumber, (int?)request.UserType);

                return Ok(new UserDtoResponse(user));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This route is used to delete an user
        /// </summary>
        /// <param name="id">Id of the User</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult Delete([FromRoute(Name = "id")] int id)
        {
            try
            {
                if (GetUserId() != id && GetUserRole() != Constants.Administrator)
                    return Forbid();

                _userService.DeleteUser(id);

                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Domain.DTOs.Request;
using GamesLoan.Domain.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GamesLoan.Api.Controllers
{
    public class LoanController : BaseApiController
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        /// <summary>
        /// This route is used to return all loaned games of the current user
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LoanDtoResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult GetLoansOfUser()
        {
            try
            {
                var loans = _loanService.GetLoansOfUser(GetUserId());
                return Ok(loans.Select(x => new LoanDtoResponse(x)));
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

        ///<summary>
        /// This route is used to loaned a game
        /// </summary>
        /// <param name="gameNames">Name of the games who will be loaned</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<LoanDtoResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult CreateLoan([FromBody] List<string> gameNames)
        {
            try
            {
                var loans = _loanService.CreateLoans(GetUserId(), gameNames);
                return Created($"BorrowGameId: {loans.Select(c => c.Id).ToList()}", loans.Select(x => new LoanDtoResponse(x)));
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

        ///<summary>
        /// This route is used to return a game
        /// </summary>
        /// <param name="ids">Ids of the loans that will be returned</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LoanDtoResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult ReturnGame([FromBody] List<int> ids)
        {
            try
            {
                var loans = _loanService.ReturnGames(GetUserId(), ids);
                return Ok(loans.Select(x => new LoanDtoResponse(x)));
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

        ///<summary>
        /// This route is used to loaned a game
        /// </summary>
        /// <param name="request">
        /// UserEmail = Email of the Friend
        /// <br /> 
        /// Ids = Id of the Games that will be loaned
        /// </param>
        [HttpPost("CreateLoanWithFriendEmail")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<LoanDtoResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult CreateLoanWithFriendEmail([FromBody] LoanDtoRequest request)
        {
            try
            {
                var loans = _loanService.CreateLoansWithFriendEmail(request.UserEmail, request.Ids);
                return Created($"BorrowGameId: {loans.Select(c => c.Id).ToList()}", loans.Select(x => new LoanDtoResponse(x)));
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

        ///<summary>
        /// This route is used to return a game
        /// </summary>
        /// <param name="request">
        /// UserEmail = Email of the Friend
        /// <br /> 
        /// Ids = Id of the Games that will be returned
        /// </param>
        [HttpPut("ReturnGamesWithFriendEmail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LoanDtoResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult ReturnGamesWithFriendEmail([FromBody] LoanDtoRequest request)
        {
            try
            {
                var loans = _loanService.ReturnGamesWithFriendEmail(request.UserEmail, request.Ids);
                return Ok(loans.Select(x => new LoanDtoResponse(x)));
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

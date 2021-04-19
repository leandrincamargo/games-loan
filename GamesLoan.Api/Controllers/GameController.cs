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
    public class GameController : BaseApiController
    {
        private readonly IGameService _gamesService;

        public GameController(IGameService gamesService)
        {
            _gamesService = gamesService;
        }

        /// <summary>
        /// This route is used to return a game by id
        /// </summary>
        /// <param name="id">Id of the Game</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameDtoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult GetById([FromRoute(Name = "id")] int id)
        {
            try
            {
                var game = _gamesService.GetGame(id);
                return Ok(new GameDtoResponse(game));
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
        /// This route is used to return all games
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GameDtoResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var games = _gamesService.GetGames();
                return Ok(games.Select(x => new GameDtoResponse(x)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This route is used to create a game
        /// </summary>
        /// <param name="name">Name of the Game to be created</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GameDtoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult Post([FromBody] string name)
        {
            try
            {
                var game = _gamesService.CreateGame(name);
                return Created($"Game: {game.Id}", new GameDtoResponse(game));
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
        /// This route is used to edit a game
        /// </summary>
        /// <param name="id">Id of the Game</param>
        /// <param name="request">
        /// Name = Name of the Game
        /// <br /> 
        /// IsActive = If Game is Active
        /// </param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameDtoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult Put([FromRoute(Name = "id")] int id, [FromBody] GameDtoRequest request)
        {
            try
            {
                var game = _gamesService.UpdateGame(id, request.Name, request.IsActive);
                return Ok(new GameDtoResponse(game));
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
        /// This route is used to delete a game
        /// </summary>
        /// <param name="id">Id of the Game</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult Delete([FromRoute(Name = "id")] int id)
        {
            try
            {
                _gamesService.DeleteGame(id);
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

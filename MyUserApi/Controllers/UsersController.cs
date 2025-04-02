using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyUserApi.Models;
using MyUserApi.Services;

namespace MyUserApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user is null)
                return NotFound($"User with id {id} not found.");

            return Ok(user);
        }

        // GET api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var users = await _userService.GetUsersAsync(pageNumber, pageSize);
            return Ok(users);
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var created = await _userService.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid data during user creation for email: {Email}", user.Email);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during user creation");
                return StatusCode(500, "Internal server error");
            }
        }


        // PUT api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest("URL id and body id do not match.");

            try
            {
                var updated = await _userService.UpdateUserAsync(user);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Update failed: user with id {UserId} not found", id);
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Update failed: invalid data for user {UserId}", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during user update with id {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }


        // DELETE api/users/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _userService.DeleteUserAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Attempt to delete non-existing user with id {UserId}", id);
                    return NotFound($"User with id {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting user with id {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}

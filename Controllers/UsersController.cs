using Microsoft.AspNetCore.Mvc;
using SecondTimeAttempt.Models.Domain;
using SecondTimeAttempt.Models.DTO;
using SecondTimeAttempt.Services;
using System;
using System.Threading.Tasks;

namespace SecondTimeAttempt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usersDto = await _userService.GetAllUsersAsync();
            return Ok(new { Message = "Success from GetAll Action method", Data = usersDto });
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto == null)
                return NotFound(new { Message = "No User found!" });

            return Ok(new { Message = "User found", Data = userDto });
        }

        [HttpPost]
        public async Task<IActionResult> InsertSingle([FromBody] InsertUserDto insertUserDto)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                
            };

            var newUserDto = await _userService.InsertUserAsync(newUser);
            return Ok(new { Message = "User Created!", Data = newUserDto });
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateById(Guid id, [FromForm] UpdateUserDto updateUserDto)
        {
            var userToUpdate = await _userService.GetUserByIdAsync(id);
            if (userToUpdate == null)
                return NotFound(new { Message = "No User found for updating!" });

            // Map properties from updateUserDto to userToUpdate
            var updatedUserDto = await _userService.UpdateUserAsync(userToUpdate);
            return Ok(new { Message = "User Updated", Data = updatedUserDto });
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound(new { Message = "No User found for deletion!" });

            return Ok(new { Message = "User Deleted" });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecondTimeAttempt.Models.DTO;
using SecondTimeAttempt.Services;
using System;
using System.Threading.Tasks;

namespace SecondTimeAttempt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll()
        {
            var commentsDto = await _commentService.GetAllCommentsByUserIdAsync();
            return Ok(new { Message = "Successfully fetched the comments", Data = commentsDto });
        }

        [HttpGet("{id:Guid}"), Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var commentDto = await _commentService.GetCommentByIdAsync(id);
            if (commentDto == null)
            {
                return NotFound(new { Message = "No comment found!" });
            }

            return Ok(new { Message = "Comment found", Data = commentDto });
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> InsertSingle([FromBody] InsertCommentDto insertCommentDto)
        {
            var commentDto = await _commentService.InsertCommentAsync(insertCommentDto);
            return Ok( new { Message = "Comment Created", Data = commentDto });
        }

        [HttpPut("{id:Guid}"), Authorize]
        public async Task<IActionResult> UpdateById([FromRoute] Guid id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            var commentDto = await _commentService.UpdateCommentAsync(id, updateCommentDto);
            return Ok(new { Message = "Comment Updated", Data = commentDto });
        }

        [HttpDelete("{id:Guid}"), Authorize]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var isDeleted = await _commentService.DeleteCommentAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { Message = "No comment found!" });
            }

            return Ok(new { Message = "Comment Deleted" });
        }
    }
}

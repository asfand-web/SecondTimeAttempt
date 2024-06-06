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
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll()
        {
            var postsDto = await _postService.GetAllPostsAsync();
            return Ok(new { Message = "Successfully fetched posts", Data = postsDto });
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var postDto = await _postService.GetPostByIdAsync(id);
            if (postDto == null)
            {
                return NotFound(new { Message = "No post found!" });
            }

            return Ok(new { Message = "Post found", Data = postDto });
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> InsertSingle([FromBody] InsertPostDto insertPostDto)
        {
            var postDto = await _postService.InsertPostAsync(insertPostDto);
            return Ok(new { Message = "Post Created", Data = postDto });
        }

        [HttpPut("{id:Guid}"), Authorize]
        public async Task<IActionResult> UpdateById([FromRoute] Guid id, [FromBody] UpdatePostDto updatePostDto)
        {
            var postDto = await _postService.UpdatePostAsync(id, updatePostDto);
            return Ok(new { Message = "Post Updated", Data = postDto });
        }

        [HttpDelete("{id:Guid}"), Authorize]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var isDeleted = await _postService.DeletePostAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { Message = "No post found!" });
            }

            return Ok(new { Message = "Post Deleted" });
        }
    }
}

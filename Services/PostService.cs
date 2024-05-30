using AutoMapper;
using Microsoft.AspNetCore.Http;
using SecondTimeAttempt.Extensions;
using SecondTimeAttempt.Models.Domain;
using SecondTimeAttempt.Models.DTO;
using SecondTimeAttempt.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecondTimeAttempt.Services
{
    public interface IPostService
    {
        Task<List<PostDto>> GetAllPostsAsync();
        Task<PostDto> GetPostByIdAsync(Guid id);
        Task<PostDto> InsertPostAsync(InsertPostDto insertPostDto);
        Task<PostDto> UpdatePostAsync(Guid id, UpdatePostDto updatePostDto);
        Task<bool> DeletePostAsync(Guid id);
    }

    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid GetAndValidateUserId()
        {
            var userId = _httpContextAccessor.HttpContext.GetUserId();
            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("User ID not provided");
            }
            return userId;
        }

        public async Task<List<PostDto>> GetAllPostsAsync()
        {
            var userId = GetAndValidateUserId();
            var posts = await _postRepository.GetAllPostsByUserIdAsync(userId);
            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<PostDto> GetPostByIdAsync(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> InsertPostAsync(InsertPostDto insertPostDto)
        {
            var userId = GetAndValidateUserId();
            var post = _mapper.Map<Post>(insertPostDto);
            post.UserId = userId;

            var insertedPost = await _postRepository.InsertAsync(post);
            return _mapper.Map<PostDto>(insertedPost);
        }

        public async Task<PostDto> UpdatePostAsync(Guid id, UpdatePostDto updatePostDto)
        {
            var userId = GetAndValidateUserId();
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null || post.UserId != userId)
            {
                throw new UnauthorizedAccessException("User ID does not match or not provided");
            }

            _mapper.Map(updatePostDto, post);
            var updatedPost = await _postRepository.UpdateAsync(post);
            return _mapper.Map<PostDto>(updatedPost);
        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }

            return await _postRepository.DeleteAsync(id);
        }
    }
}

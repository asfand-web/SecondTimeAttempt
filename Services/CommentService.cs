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
    public interface ICommentService
    {
        Task<List<CommentDto>> GetAllCommentsByUserIdAsync();
        Task<CommentDto> GetCommentByIdAsync(Guid id);
        Task<CommentDto> InsertCommentAsync(InsertCommentDto insertCommentDto);
        Task<CommentDto> UpdateCommentAsync(Guid id, UpdateCommentDto updateCommentDto);
        Task<bool> DeleteCommentAsync(Guid id);
    }

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
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

        public async Task<List<CommentDto>> GetAllCommentsByUserIdAsync()
        {
            var userId = GetAndValidateUserId();
            var comments = await _commentRepository.GetAllCommentsByUserIdAsync(userId);
            return _mapper.Map<List<CommentDto>>(comments);
        }

        public async Task<CommentDto> GetCommentByIdAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> InsertCommentAsync(InsertCommentDto insertCommentDto)
        {
            var userId = GetAndValidateUserId();
            var comment = _mapper.Map<Comment>(insertCommentDto);

            var insertedComment = await _commentRepository.InsertAsync(comment);
            return _mapper.Map<CommentDto>(insertedComment);
        }

        public async Task<CommentDto> UpdateCommentAsync(Guid id, UpdateCommentDto updateCommentDto)
        {
            var userId = GetAndValidateUserId();
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                throw new UnauthorizedAccessException("User ID does not match or not provided");
            }

            _mapper.Map(updateCommentDto, comment);
            var updatedComment = await _commentRepository.UpdateAsync(comment);
            return _mapper.Map<CommentDto>(updatedComment);
        }

        public async Task<bool> DeleteCommentAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                throw new KeyNotFoundException("Comment not found");
            }

            return await _commentRepository.DeleteAsync(id);
        }
    }
}

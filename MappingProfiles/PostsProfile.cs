using AutoMapper;
using SecondTimeAttempt.Models.Domain;
using SecondTimeAttempt.Models.DTO;
using System.Linq;

namespace SecondTimeAttempt.MappingProfiles
{
    public class PostsProfile : Profile
    {
        public PostsProfile()
        {
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Select(c => new CommentDto
                {   
                    Text = c.Text,
                    Rating = c.Rating,
                    PostId = c.PostId
                })));

            CreateMap<PostDto, Post>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Select(c => new Comment
                {   
                    Text = c.Text,
                    Rating = c.Rating,
                    PostId = c.PostId
                })));

            CreateMap<InsertPostDto, Post>().ReverseMap();
            CreateMap<UpdatePostDto, Post>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<InsertCommentDto, Comment>().ReverseMap();
            CreateMap<UpdateCommentDto, Comment>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<InsertUserDto, User>().ReverseMap();
            CreateMap<UpdateUserDto, User>().ReverseMap();

        }
    }
}


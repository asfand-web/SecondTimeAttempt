using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecondTimeAttempt.Data;
using SecondTimeAttempt.MappingProfiles;
using SecondTimeAttempt.Repositories;
using SecondTimeAttempt.Services;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SecondDatabase"))
);

builder.Services.AddAutoMapper(typeof(PostsProfile));

// Add repository services
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add application services
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<TokenService>();

// Configure JWT authentication
var key = builder.Configuration.GetSection("Jwt:SecretKey").Value;
if (string.IsNullOrEmpty(key))
    throw new ArgumentNullException("Secret Key cannot be null");

var securityKey = Encoding.UTF8.GetBytes(key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(securityKey),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable authentication and authorization and VerificationStatusMiddleWare middleware
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<VerificationStatusMiddleware>();

app.MapControllers();

// Run the application
app.Run();

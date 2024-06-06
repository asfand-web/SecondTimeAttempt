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
        // Configure JSON serialization options here
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with security definitions for JWT authentication
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

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SecondDatabase"))
);

// Configure AutoMapper with the mapping profiles
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
// Add token service as a singleton
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

// Add HttpContextAccessor for accessing HTTP context
builder.Services.AddHttpContextAccessor();

// Register EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger only in development
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<VerificationStatusMiddleware>();
// Map controller routes
app.MapControllers();

// Run the application
app.Run();

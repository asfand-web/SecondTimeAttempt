using SecondTimeAttempt.Services;

public class VerificationStatusMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<VerificationStatusMiddleware> _logger;

    public VerificationStatusMiddleware(RequestDelegate next, ILogger<VerificationStatusMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IUserService userService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                _logger.LogInformation($"Checking verification status for user ID: {userId}");
                var verificationStatus = await userService.GetUserVerificationStatusByIdAsync(userId);
                _logger.LogInformation($"Verification status for user ID {userId}: {verificationStatus}");

                if (verificationStatus == "Pending")
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new { Message = "User is inactive. Please verify your email." });
                    return;
                }
            }
        }

        await _next(context);
    }
}

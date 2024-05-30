namespace SecondTimeAttempt.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetUserId(this HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.FindFirst("UserID");
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
        }
    }
}

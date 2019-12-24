using Microsoft.AspNetCore.Builder;

namespace FitnessTracker.Common.Middleware
{
    public static class RequestTimingsExtention
    {
        public static IApplicationBuilder UseRequestTimings(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestTimingsMiddleWare>();
            return app;
        }
    }
}
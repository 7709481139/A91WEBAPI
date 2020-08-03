using Microsoft.AspNetCore.Http;

namespace A91WEBAPI.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Allow-Origine", "Application-Error");
            response.Headers.Add("Access-Controll-Allow-Origin","*");
        }
    }
}
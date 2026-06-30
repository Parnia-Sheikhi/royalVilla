using static RoyalVillaWeb.SD;

namespace RoyalVillaWeb.Models
{
    public class ApiRequest  // send the web request to api project
    {
        public ApiType ApiType { get; set; } = ApiType.GET;

        public string? Url { get; set; }

        public object? Data { get; set; }

        public string? Token { get; set; }
    }
}

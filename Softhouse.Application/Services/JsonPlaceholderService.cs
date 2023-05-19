using Softhouse.Domain;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace Softhouse.Application.Services
{
    public class JsonPlaceholderService : IJsonPlaceholderService
    {
        private readonly HttpClient _httpClient;

        public JsonPlaceholderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<Comments>> GetCommentsAsync()
        {
            var response = _httpClient.GetAsync(
                "https://jsonplaceholder.typicode.com/comments").Result;

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<IEnumerable<Comments>>(responseAsString);
        }

        public async Task<List<Comments>> GetCommentsById(int postId)
        {
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/comments?postId={postId}");
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<List<Comments>>(responseAsString);
        }

    }
}

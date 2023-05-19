using Softhouse.Domain;

namespace Softhouse.Application.Services
{
    public interface IJsonPlaceholderService
    {  
        Task<IEnumerable<Comments>> GetCommentsAsync();
        Task<List<Comments>> GetCommentsById(int postId);
    }
}

using MediatR;
using Softhouse.Application.Services;
using Softhouse.Domain;

namespace Softhouse.Application.Queries
{
    public class GetCommentsByPostIdQueryHandler : IRequestHandler<GetCommentsByPostIdQuery, List<Comments>>
    {
        private readonly IJsonPlaceholderService _jsonPlaceholderService;

        public GetCommentsByPostIdQueryHandler(IJsonPlaceholderService jsonPlaceholderService)
        {
            _jsonPlaceholderService = jsonPlaceholderService;
        }
        public async Task<List<Comments>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
        {
            return await _jsonPlaceholderService.GetCommentsById(request.PostId);
        }
    }
}

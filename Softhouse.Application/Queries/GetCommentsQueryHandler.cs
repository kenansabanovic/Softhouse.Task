using MediatR;
using Softhouse.Application.Services;
using Softhouse.Domain;

namespace Softhouse.Application.Queries
{
    public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, IEnumerable<Comments>>
    {
        private readonly IJsonPlaceholderService _jsonPlaceholderService;

        public GetCommentsQueryHandler(IJsonPlaceholderService jsonPlaceholderService)
        {
            _jsonPlaceholderService = jsonPlaceholderService;
        }

        public async Task<IEnumerable<Comments>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            return await _jsonPlaceholderService.GetCommentsAsync();
        }
    }
}

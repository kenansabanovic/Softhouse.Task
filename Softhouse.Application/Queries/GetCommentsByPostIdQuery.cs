using MediatR;
using Softhouse.Domain;

namespace Softhouse.Application.Queries
{
    public class GetCommentsByPostIdQuery : IRequest<List<Comments>>
    {
        public int PostId { get; set; }
    }
}

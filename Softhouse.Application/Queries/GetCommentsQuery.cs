using MediatR;
using Softhouse.Domain;

namespace Softhouse.Application.Queries
{
    public class GetCommentsQuery : IRequest<IEnumerable<Comments>>
    {
    }
}

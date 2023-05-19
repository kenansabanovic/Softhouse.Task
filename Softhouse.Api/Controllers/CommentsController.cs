using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Softhouse.Application.Queries;

namespace Softhouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetComments()
        {
            return Ok(await _mediator.Send(new GetCommentsQuery()));
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentByPostId([FromQuery] int postId)
        {
            return Ok(await _mediator.Send(new GetCommentsByPostIdQuery
            {
                PostId = postId
            }));
        }
    }
}

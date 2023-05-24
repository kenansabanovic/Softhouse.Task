using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Softhouse.Api.Dtos;
using Softhouse.Application.Queries;

namespace Softhouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyCorsPolicy")]
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
        [Route("updated")]
        public async Task<IActionResult> GetUpdatedComments()
        {
            try
            {
                var projectDirectory = Directory.GetCurrentDirectory();

                var filePath = Path.Combine(projectDirectory, "Comments.json");

                var existingComments = ReadCommentsFromJson(filePath);

                var json = JsonConvert.SerializeObject(existingComments);

                System.IO.File.WriteAllText(filePath, json);
                await Task.Delay(200);
                return Created("api/mycontroller", existingComments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the comment: {ex.Message}");
            }
        }

        [HttpGet]

        public async Task<IActionResult> GetCommentByPostId([FromQuery] int postId)
        {
            return Ok(await _mediator.Send(new GetCommentsByPostIdQuery
            {
                PostId = postId
            }));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateComment([FromBody] IList<CommentDto> comments)
        {
            try
            {
                var projectDirectory = Directory.GetCurrentDirectory();

                var filePath = Path.Combine(projectDirectory, "Comments.json");

                var json = JsonConvert.SerializeObject(comments);

                System.IO.File.WriteAllText(filePath, json);
                await Task.Delay(200);
                return Created("api/mycontroller", comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the comment: {ex.Message}");
            }
        }

        private List<CommentDto> ReadCommentsFromJson(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);

                var comments = JsonConvert.DeserializeObject<List<CommentDto>>(json);

                return comments;
            }

            return new List<CommentDto>();
        }
    }

}
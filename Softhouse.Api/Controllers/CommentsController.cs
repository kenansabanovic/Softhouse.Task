using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        //[Authorize]
        public async Task<IActionResult> GetUpdatedComments()
        {
            try {
                string projectDirectory = Directory.GetCurrentDirectory();

                string filePath = Path.Combine(projectDirectory, "Comments.json");

                // Read existing comments from the JSON file
                var existingComments = ReadCommentsFromJson(filePath);
                // Serialize the updated comments list to JSON
                string json = JsonConvert.SerializeObject(existingComments);

                // Save the JSON string to the file
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
                string projectDirectory = Directory.GetCurrentDirectory();

                string filePath = Path.Combine(projectDirectory, "Comments.json");

                string json = JsonConvert.SerializeObject(comments);

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
                string json = System.IO.File.ReadAllText(filePath);

                var comments = JsonConvert.DeserializeObject<List<CommentDto>>(json);

                return comments;
            }

            // If the file doesn't exist, return an empty list
            return new List<CommentDto>();
        }
    }

    public class CommentDto
    {
        public int PostId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }
}
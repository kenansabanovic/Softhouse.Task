using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Softhouse.Application.Queries;
using System.IO;
using System.Reflection;

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

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateComment([FromBody] CommentDto comment)
        {
            try
            {
                string projectDirectory = Directory.GetCurrentDirectory();

                // Build the full path to the file
                string filePath = Path.Combine(projectDirectory, "Comments.json");

                // Read existing comments from the JSON file
                var existingComments = ReadCommentsFromJson(filePath);

                // Add the new comment to the existing list
                existingComments.Add(comment);

                // Serialize the updated comments list to JSON
                string json = JsonConvert.SerializeObject(existingComments);

                // Save the JSON string to the file
                System.IO.File.WriteAllText(filePath, json);
                await Task.Delay(200);
                return Created("api/mycontroller", comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the comment: {ex.Message}");
            }
        }

        private List<CommentDto> ReadCommentsFromJson(string filePath)
        {
            // Check if the JSON file exists
            if (System.IO.File.Exists(filePath))
            {
                // Read the JSON file content
                string json = System.IO.File.ReadAllText(filePath);

                // Deserialize the JSON into a list of CommentDto objects
                var comments = JsonConvert.DeserializeObject<List<CommentDto>>(json);

                // Return the list of comments
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
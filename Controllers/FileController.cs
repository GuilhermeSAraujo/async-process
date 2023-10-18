using async_process.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Channels;

namespace async_process.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly Channel<Todo> _createTodoChannel;
        private readonly ILogger<FileController> _logger;

        public FileController(Channel<Todo> createTodoChannel, ILogger<FileController> logger)
        {
            _createTodoChannel = createTodoChannel;
            _logger = logger;
        }

        [HttpPost(Name = "process")]
        public async Task<IActionResult> Post()
        {
            var json = System.IO.File.ReadAllText("Todos.json");
            var todos = JsonSerializer.Deserialize<IEnumerable<Todo>>(json);

            foreach (var todo in todos)
            {
                await _createTodoChannel.Writer.WriteAsync(todo);
            }

            _logger.LogInformation("Retornou Ok");

            return Ok();
        }
    }
}

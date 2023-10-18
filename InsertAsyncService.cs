using async_process.Models;
using System.Threading.Channels;

namespace async_process
{
    public class InsertAsyncService : BackgroundService
    {
        private readonly Channel<Todo> _createTodoChannel;
        private readonly ILogger<InsertAsyncService> _logger;

        public InsertAsyncService(
            Channel<Todo> createTodoChannel,
            ILogger<InsertAsyncService> logger)
        {
            _createTodoChannel = createTodoChannel;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var todo in _createTodoChannel.Reader.ReadAllAsync())
                {
                    _logger.LogInformation($"{todo.id} {todo.userId} {todo.title}");
                }
            }
        }
    }
}

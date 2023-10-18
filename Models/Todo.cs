namespace async_process.Models
{
    public record Todo(
        int userId,
        int id,
        string title,
        bool completed
    );
}

using LiteDB;

namespace TariefasWebApi.Models
{
    public class Todo(string title, string? description, Guid userId, bool isCompleted)
    {
        public ObjectId Id { get; set; } = ObjectId.NewObjectId();
        public int TaskId { get; set; }
        public Guid UserId { get; set; } = userId;
        public required string Title { get; set; } = title;
        public string? Description { get; set; } = description;
        public bool IsCompleted { get; set; } = isCompleted;
    }
}

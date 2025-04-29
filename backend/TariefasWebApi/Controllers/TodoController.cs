using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TariefasWebApi.Models;
using TariefasWebApi.Data;

namespace TariefasWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly LiteDatabase context = TariefasDBContext.Context;

        [HttpGet("{id:guid}")]
        public IActionResult GetAllTodosFromUser(Guid id)
        {
            var todos = context.GetCollection<Todo>("todos");
            var userTodos = todos.Find(x => x.UserId == id).ToList();
            if (userTodos == null || userTodos.Count == 0) return NotFound("No todos found for this user.");
            return Ok(userTodos);
        }

        [HttpPost]
        public IActionResult AddNewTodo(Todo todo)
        {
            var todos = context.GetCollection<Todo>("todos");
            // find last todo from a user
            var userTodos = todos.Find(x => x.UserId == todo.UserId).ToList();
            var last = userTodos.OrderByDescending(x => x.TaskId).FirstOrDefault();
            todo.TaskId = last == null ? 1 : last.TaskId + 1;

            todos.Insert(todo);
            return CreatedAtAction(nameof(GetAllTodosFromUser), new { id = todo.UserId }, todo);
        }

        [HttpPut("edit/{userId:guid}-{taskId:int}")]
        public IActionResult UpdateTodoFromUser(Guid userId, int taskId, Todo todo)
        {
            var todos = context.GetCollection<Todo>("todos");
            var existingTodo = todos.FindOne(x => x.UserId == userId && x.TaskId == taskId);
            if (existingTodo == null) return NotFound("Todo not found.");
            // Update the todo in one line
            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsCompleted = todo.IsCompleted;
            todos.Update(existingTodo);
            return Ok(existingTodo);
        }

        [HttpDelete("delete/{userId:guid}-{taskId:int}")]
        public IActionResult DeleteTodoFromUser(Guid userId, int taskId)
        {
            var todos = context.GetCollection<Todo>("todos");
            var existingTodo = todos.FindOne(x => x.UserId == userId && x.TaskId == taskId);
            if (existingTodo == null) return NotFound("Todo not found.");
            todos.DeleteMany(x => x.UserId == userId && x.TaskId == taskId);
            return Ok("Todo deleted successfully.");
        }
    }
}

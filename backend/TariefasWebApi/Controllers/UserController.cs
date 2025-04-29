using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TariefasWebApi.Models;
using TariefasWebApi.Data;

namespace TariefasWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LiteDatabase context = TariefasDBContext.Context;

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(context.GetCollection<User>("users").FindAll().ToList());
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            var users = context.GetCollection<User>("users");
            if (users.FindOne(u => u.Email == user.Email) != null) return BadRequest("This email is already in use");
            users.Insert(user);
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetUserById(Guid id)
        {
            var users = context.GetCollection<User>("users");
            var user = users.FindOne(users => users.Id == id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        [HttpGet("Auth/{email}-{password}")]
        public IActionResult CheckUser(string email, string password)
        {
            var users = context.GetCollection<User>("users");
            var user = users.FindOne(users => users.Email == email);
            if (user == null) return NotFound("User not found");
            if (user.Password != password) return BadRequest("Wrong Password");
            return Ok(user.Id);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            var users = context.GetCollection<User>("users");
            var user = users.FindOne(users => users.Id == id);
            if (user == null) return NotFound("User not found");
            users.Delete(user.Id);
            return Ok("User deleted");
        }
    }
}

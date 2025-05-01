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
            try
            {
                var users = context.GetCollection<User>("users");
                if (users.FindOne(u => u.Email == user.Email) != null) return BadRequest("This email is already in use");
                users.Insert(user);
                Console.WriteLine($"New User Created!\nUser Email: {user.Email}");
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
            
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetUserById(Guid id)
        {
            try
            {
                var users = context.GetCollection<User>("users");
                var user = users.FindOne(users => users.Id == id);
                if (user == null) return NotFound("User not found");
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }         
        }

        [HttpPost("Auth")]
        public IActionResult CheckUser([FromBody] User login)
        {
            var users = context.GetCollection<User>("users");
            var user = users.FindOne(users => users.Email == login.Email);
            if (user == null) return AddUser(login);
            if (user.Password != login.Password) return BadRequest("Wrong Password");
            Console.WriteLine($"User Authenticated!\n{user}");
            return Ok(user);
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

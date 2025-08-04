using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UsersController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        //[HttpPost]
        //public IActionResult Create(User user)
        //{
        //    _context.Users.Add(user);
        //    _context.SaveChanges();
        //    return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        //}

        [HttpPost]
        public IActionResult Create(User user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("db-test")]
        public async Task<IActionResult> TestDbConnection()
        {
            try
            {
                using var connection = new SqlConnection("Server=user-sqlserver;Database=UserDB;User Id=sa;Password=Your_strong_password123;TrustServerCertificate=True;");
                await connection.OpenAsync();
                return Ok("✅ Connected to DB!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ DB connection failed: {ex.Message}");
            }
        }

    }
}

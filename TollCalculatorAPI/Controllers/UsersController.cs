using Microsoft.AspNetCore.Mvc;
using TollCalculatorAPI.Models;
using TollCalculatorAPI.Services;

namespace TollCalculatorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;


        public UsersController(IUserRepository repo)
        {
            _repo = repo;

        }

        //Hämtar mina användare och deras fordon från mockdatan
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            var users = _repo.GetUsers();
            var result = users.Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Vehicles = u.Vehicles,

            }).ToList();

            return result;
        }

        //Hämtar en specifik användare med fordon baserat på namn
        [HttpGet("byname/{name}")]
        public ActionResult<User> GetUserByName(string name)
        {
            var user = _repo.GetUserByName(name);
            if (user == null)
            {
                return NotFound();
            }
            var userResult = new User
            {
                Id = user.Id,
                Name = user.Name,
                Vehicles = user.Vehicles,

            };

            return userResult;
        }
    }
}


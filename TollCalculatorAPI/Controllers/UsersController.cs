using Microsoft.AspNetCore.Mvc;
using TollCalculatorAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;
    

    public UsersController(IUserRepository repo)
    {
        _repo = repo;
        
    }

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
}


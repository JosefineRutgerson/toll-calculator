using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculatorAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users;

    public UserRepository(IWebHostEnvironment env)
    {
        // Get the absolute path to your JSON file
        var filePath = Path.Combine(env.ContentRootPath, "Data", "mockdata.json");

        if (File.Exists(filePath))
        {
            // Read JSON from file
            var json = File.ReadAllText(filePath);

            // Convert JSON into List<User>
            _users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }
        else
        {
            _users = new List<User>(); // fallback if no file
        }
    }

    public List<User> GetUsers() => _users;

    public User? GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }
    
    public User? GetUserByName(string name)
    {
        return _users.FirstOrDefault(u => u.Name == name);;
    }
        
      

}
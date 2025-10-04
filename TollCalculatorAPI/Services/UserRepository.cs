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
        // Bestäm sökvägen till mockdata.json-filen
        var filePath = Path.Combine(env.ContentRootPath, "Data", "mockdata.json");

        if (File.Exists(filePath))
        {
            // läs in JSON-filen
            var json = File.ReadAllText(filePath);

            // konvertera JSON till lista av User-objekt
            _users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }
        else
        {
            _users = new List<User>(); // om filen inte finns, initiera en tom lista
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
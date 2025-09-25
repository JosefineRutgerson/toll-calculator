using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculatorAPI.Models;

public interface IUserRepository
{
    List<User> GetUsers();
    User? GetUserById(int id);
    User? GetUserByName(string name);
}
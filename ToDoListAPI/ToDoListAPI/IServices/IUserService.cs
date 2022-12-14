using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.IService
{
    public interface IUserService
    {
        Task<User> CreateUser(User user);
        Task<string> Login(string username, string pass);
    }
}

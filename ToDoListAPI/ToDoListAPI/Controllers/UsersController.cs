using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.IService;
using ToDoListAPI.Models;

namespace ToDoListAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRequest userDTO)
        {
            var user = new User();
            //Chuyen dto sang user
            user.ConvertFormUserDTO(userDTO);
            var resService = await userService.CreateUser(user);

            if (resService == null)
            {
                return NotFound(new { Mess = "Username available" });
            }
            else
            {
                return Ok(new { Mess = "Successful" });
            }
        }
        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<IActionResult> Login(string username, string pass)
        {
            var resService = await userService.Login(username, pass);

            if (resService == null)
            {
                return NotFound(new { Mess = "Login failed" });
            }
            else
            {
                return Ok(new { Token = resService });
            }
        }
    }
}
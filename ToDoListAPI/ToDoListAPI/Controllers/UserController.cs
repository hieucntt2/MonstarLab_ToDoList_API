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
    public class UserController : ControllerBase
    {
        private IUserServices userService;
        public UserController(IUserServices userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(string token)
        {
            //Kiểm tra token 
            var verifyToken = userService.VerifyToken(token);
            if (verifyToken != null)
            {
                var listUser = await userService.GetAll();
                return Ok(new { Mess = "Success" , listUser});
            }
            else
            {
                return Unauthorized(new { Mess = "Chưa đăng nhập" });
            }

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
                return NotFound(new { Mess = "Tên đăng nhập đã tồn tại" });
            }
            else
            {
                return Ok(new { Mess = "Tạo thành công" });
            }
        }
        [AllowAnonymous]
        [HttpGet("Login")]
        public async Task<IActionResult> Login(string username, string pass)
        {
            var resService = await userService.Login(username, pass);

            if (resService == null)
            {
                return NotFound(new { Mess = "Đăng nhập thất bại" });
            }
            else
            {
                return Ok(new { Token = resService });
            }
        }

    }
}

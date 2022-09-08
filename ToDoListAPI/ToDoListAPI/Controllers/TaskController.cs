using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoListAPI.IService;
using ToDoListAPI.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskServices taskService;
        public TaskController(ITaskServices taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTask()
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var listTask = await taskService.GetAllTask(userId);
            return Ok(listTask);
        }
        [HttpGet("task/{status}")]
        public async Task<IActionResult> GetByStatus(bool status)
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var listTask = await taskService.GetByStatus(userId, status);
            return Ok(listTask);
        }
        [HttpGet("{date}")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var listTask = await taskService.GetByDate(userId, date);
            return Ok(listTask);
        }

        [HttpPost("Complete")]
        public async Task<IActionResult> CompleteTask(List<int> listIdTask)
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var resService = await taskService.CompleteTasks(userId, listIdTask);
            if (resService != null)
            {
                return NotFound(new { Mess = "Invalid ID" });
            }
            return Ok(new { Mess = "Successful" });
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateTask(TaskRequest taskDTO)
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var task = new Models.Task();
            //Chuyen dto sang user
            task.ConvertFormTaskDTO(taskDTO);
            var resService = await taskService.CreateTask(userId, task);
            if (resService != null)
            {
                return Ok(new { Mess = "Create Success", resService });
            }
            return NotFound(new { Mess = "Unsuccessful" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var resService = await taskService.DeleteTask(userId, id);
            if (resService == null)
            {
                return NotFound(new { Mess = "Delete Failed" });
            }
            return Ok(new { Mess = "Delete Success" });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Models.Task task)
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var resService = await taskService.UpdateTask(userId, id, task);
            if (resService == null)
            {
                return NotFound(new { Mess = "Update success" });
            }
            return Ok(new { Mess = "Successful" });
        }
    }
}
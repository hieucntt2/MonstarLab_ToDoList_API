using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("/GetAllTask")]
        public async Task<IActionResult> GetAllTask(int userId)
        {
            var principal = HttpContext.User;
            var iduser = principal?.Claims?.SingleOrDefault(p => p.Type == "UserName")?.Value;

            var listTask = await taskService.GetAllTask(userId);
            return Ok(listTask);
        }
        [HttpGet("/Get-by-status")]
        public async Task<IActionResult> GetByStatus(int userId, bool status)
        {
            var listTask = await taskService.GetByStatus(userId, status);
            return Ok(listTask);
        }
        [HttpGet("/Get-by-date")]
        public async Task<IActionResult> GetByDate(int userId, DateTime date)
        {
            var listTask = await taskService.GetByDate(userId, date);
            return Ok(listTask);
        }

        [HttpPost("/Complete-Tasks")]
        public async Task<IActionResult> CompleteTask(int userId, List<int> listIdTask)
        {
            var resService = await taskService.CompleteTasks(userId, listIdTask);
            if (resService != null)
            {
                return NotFound(new { Mess = "Invalid ID" });
            }
            return Ok(new { Mess = "Successful" });
        }
        [HttpPost("/Create-Task")]
        public async Task<IActionResult> CreateTask(int userId, TaskDTO taskDTO)
        {
            var task = new Models.Task();
            //Chuyen dto sang user
            task.ConvertFormTaskDTO(taskDTO);
            var resService = await taskService.CreateTask(userId,task);
            if (resService != null)
            {
                return Ok(new { Mess = "Create Success", resService });
            }
            return NotFound(new { Mess = "Unsuccessful" });
        }

        [HttpDelete("/Delete-Tasks")]
        public async Task<IActionResult> DeleteTask(int userId, int id)
        {
            var resService = await taskService.DeleteTask(userId, id);
            if (resService == null)
            {
                return NotFound(new { Mess = "Delete Failed" });
            }
            return Ok(new { Mess = "Delete Success" });
        }
        [HttpPut("/Update-Task")]
        public async Task<IActionResult> UpdateTask(int userId, int id, Models.Task task)
        {
            var resService = await taskService.UpdateTask(userId, id, task);
            if (resService == null)
            {
                return NotFound(new { Mess = "Update success" });
            }
            return Ok(new { Mess = "Successful" });
        }
    }
}
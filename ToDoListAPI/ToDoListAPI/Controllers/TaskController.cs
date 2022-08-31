using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListAPI.IService;
using ToDoListAPI.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase

    {
        private ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet("/GetAllTask")]
        public async Task<IActionResult> GetAllTask(int userId)
        {
            var listTask = await taskService.GetAllTask(userId);
            return Ok(listTask);
        }
        [HttpGet("/Get-by-status")]
        public async Task<IActionResult> GetByStatus(bool status)
        {
            var listTask = await taskService.GetByStatus(status);
            return Ok(listTask);
        }
        [HttpGet("/Get-by-date")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var listTask = await taskService.GetByDate(date);
            return Ok(listTask);
        }

        [HttpPost("/Complete-Tasks")]
        public async Task<IActionResult> CompleteTask(List<int> listIdTask)
        {
            var resService = await taskService.CompleteTasks(listIdTask);
            if (resService == "01")
            {
                return NotFound(new { Mess = "Invalid ID" });
            }
            return Ok(new { Mess = "Successful" });
        }
        [HttpPost("/Create-Task")]
        public async Task<IActionResult> CreateTask(TaskDTO taskDTO)
        {
            var task = new Models.Task();
            //Chuyen dto sang user
            task.ConvertFormTaskDTO(taskDTO);
            var resService = await taskService.CreateTask(task);
            if (resService != null)
            {
                return Ok(new { Mess = "Create Success", resService });
            }
            return NotFound(new { Mess = "Unsuccessful" });
        }

        [HttpDelete("/Delete-Tasks")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var resService = await taskService.DeleteTask(id);
            if (resService == null)
            {
                return NotFound(new { Mess = "Delete Failed" });
            }
            return Ok(new { Mess = "Delete Success" });
        }
        [HttpPut("/Update-Task")]
        public async Task<IActionResult> UpdateTask(int id, Models.Task task)
        {
            var resService = await taskService.UpdateTask(id, task);
            if (resService == "01")
            {
                return NotFound(new { Mess = "Update success" });
            }
            return Ok(new { Mess = "Successful" });
        }
    }
}
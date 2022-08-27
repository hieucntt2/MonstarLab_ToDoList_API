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
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
       
    {
        private ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var listTask = await taskService.GetAll();
            return Ok(listTask);
        }
        [HttpGet("Get-by-status")]
        public async Task<IActionResult> GetByStatus(bool status)
        {
            var listTask = await taskService.GetByStatus(status);
            return Ok(listTask);
        }
        [HttpGet("Get-by-date")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var listTask = await taskService.GetByDate(date);
            return Ok(listTask);
        }

        [HttpPost("Complete-Tasks")]
        public async Task<IActionResult> CompleteTask(List<int> listIdTask)
        {
            var resService = await taskService.CompleteTasks(listIdTask);
            if (resService == "01")
            {
                return NotFound(new { Mess = "ID không hợp lệ" });
            }
            else
            {
                return Ok(new { Mess = "Thành công" });
            }
        }
        [HttpPost("Create-Task")]
        public async Task<IActionResult> CreateTask(Models.Task task)
        {
            var resService = await taskService.CreateTask(task);
            if (resService == "00")
            {
                return Ok(new { Mess = "Tạo thành công" });
            }
            else
            {
                return NotFound(new { Mess = "Không thành công" });
            }
               
        }

        [HttpDelete("Delete-Tasks")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var resService = await taskService.DeleteTask(id);
            if(resService == "01")
            {
                return NotFound(new { Mess = "Xóa không thành công" });
            }
            else
            {
                return Ok(new { Mess = "Xóa thành công" });
            }
        }
        [HttpPut("Update-Task")]
        public async Task<IActionResult> UpdateTask(int id, Models.Task task)
        {
            var resService = await taskService.UpdateTask(id,task);
            if (resService == "01")
            {
                return NotFound(new { Mess = "Update không thành công" });
            }
            else
            {
                return Ok(new { Mess = "Thành công" });
            }
        }

    }
}
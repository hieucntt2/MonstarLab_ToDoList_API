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
    public class TasksController : ControllerBase
    {
        private ITaskService taskService;
        private IMapper _mapper;
        public TasksController(ITaskService taskService,IMapper mapper)
        {
            this.taskService = taskService;
            _mapper = mapper;
        }

        [HttpGet("{status}")]
        public async Task<IActionResult> GetTask(bool status, DateTime date)
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var listTask = await taskService.GetTask(userId, status, date);
            return Ok(listTask);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteTask(List<int> listIdTask)
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var resService = await taskService.CompleteTasks(userId, listIdTask);
            return Ok(new { Mess = "Successful", resService });
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateTask(TaskRequest taskDTO)
        {
            var userId = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var task = new Models.Task();
            //Create a map
            task = _mapper.Map<Models.Task>(taskDTO);
            var resService = await taskService.CreateTask(userId, taskDTO);
            if (resService == null)
            {
                return NotFound(new { Mess = "Unsuccessful" });
            }
            return Ok(new { Mess = "Create Success", resService });
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
        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskRequest taskDTO)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            var resService = await taskService.UpdateTask(userId, taskDTO);
            if (resService == null)
            {
                return NotFound(new { Mess = "Update Unsuccessful" });
            }
            return Ok(new { Mess = "Successful" });
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.IService;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public class TaskService : ITaskService
    {
        private MyDBContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(MyDBContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TaskRequest>> GetTask(int userId, bool status, DateTime date)
        {
            //return await _context.Tasks.ToListAsync();
            _logger.LogInformation("get all task");
            if (status != null && date == null)
            {
                return await _context.Tasks.Where(x => x.UserId == userId && x.Status == status).Select(x => taskDTO(x)).ToListAsync();
            }
            if (status == null && date != null)
            {
                return await _context.Tasks.Where(x => x.UserId == userId && x.CreateAt == date).Select(x => taskDTO(x)).ToListAsync();
            }
            return await _context.Tasks.Where(x => x.UserId == userId && x.CreateAt == date && x.Status == status).Select(x => taskDTO(x)).ToListAsync();

        }

        public async Task<string> CompleteTasks(int userId, List<int> listIdTask)
        {
            using var transaction = _context.Database.BeginTransaction();
            var Task = await _context.Tasks.Where(x => x.UserId == userId && listIdTask.Contains(x.Id) == true).ToListAsync();
            foreach (var item in Task)
            {
                item.Status = true;
                _context.Update(item);
            }
            await _context.SaveChangesAsync();
            transaction.Commit();
            return null;

        }
        public async Task<TaskRequest> CreateTask(int userId, TaskRequest taskDTO)
        {
            var cateId = await _context.TaskCategories.Where(x => taskDTO.CateId == x.CateId).FirstOrDefaultAsync();
            if(cateId != null)
            {
                return null;
            }
            Models.Task task = new Models.Task();
            task.CateId = taskDTO.CateId;
            task.UserId = userId;
            task.Name = taskDTO.Name;
            task.Description = taskDTO.Description;
            task.ExecAt = taskDTO.ExecAt;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return taskDTO;
        }
        public async Task<string> DeleteTask(int userId, int Id)
        {
            var task = await _context.Tasks.Where(x => x.UserId == userId && x.Id == Id).FirstAsync();
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return null;
        }
        public async Task<TaskRequest> UpdateTask(int userId, TaskRequest taskDTO)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var task = _context.Tasks.FirstOrDefault(c => c.Id == taskDTO.Id);
                task.CateId = taskDTO.CateId;
                task.UserId = userId;
                task.Name = taskDTO.Name;
                task.Description = taskDTO.Description;
                task.ExecAt = taskDTO.ExecAt;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            return taskDTO;
        }

        private static TaskRequest taskDTO(Models.Task task) =>
           new TaskRequest
           {
               Id = task.Id,
               Name = task.Name,
               Description = task.Description,
               ExecAt = task.ExecAt,
               CateId = task.CateId
           };
    }
}
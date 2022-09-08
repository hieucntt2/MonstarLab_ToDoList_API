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
    public class TaskService : ITaskServices
    {
        private MyDBContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(MyDBContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TaskRequest>> GetAllTask(int userId)
        {
            //return await _context.Tasks.ToListAsync();
            _logger.LogInformation("get all task");
            return await _context.Tasks
                .Where(x => x.UserId == userId)
                .Select(x => taskDTO(x))
                .ToListAsync();
        }
        public async Task<List<TaskRequest>> GetByStatus(int userId, bool status)
        {
            return await _context.Tasks.Where(x => x.Status == status).Select(x => taskDTO(x)).ToListAsync();
        }
        public async Task<List<TaskRequest>> GetByDate(int userId, DateTime date)
        {
            return await _context.Tasks.Where(x => x.CreateAt == date).Select(x => taskDTO(x)).ToListAsync();
        }
        public async Task<string> CompleteTasks(int userId, List<int> listIdTask)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                //Duyệt listIDTask và lấy ra Task tương ứng
                var Task = await _context.Tasks.Where(x => listIdTask.Contains(x.Id) == true).ToListAsync();
                foreach (var item in Task)
                {
                    item.Status = true;
                    _context.Update(item);
                }
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            return null;

        }
        public async Task<Models.Task> CreateTask(int userId, Models.Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }
        public async Task<string> DeleteTask(int userId, int Id)
        {
            var task = await _context.Tasks.FindAsync(Id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return null;
        }
        public async Task<Models.Task> UpdateTask(int userId, int Id, Models.Task task)
        {
            if (Id != task.Id)
            {
                return null;
            }
            _context.Entry(task).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return task;
        }
        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
        private static TaskRequest taskDTO(Models.Task task) =>
           new TaskRequest
           {
               Name = task.Name,
               Description = task.Description,
               Status = task.Status,
               ExecAt = task.ExecAt,
               CreateAt = task.CreateAt,
               UserId = task.UserId,
               CateId = task.CateId
           };
    }
}
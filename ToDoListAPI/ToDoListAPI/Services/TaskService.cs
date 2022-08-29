using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public TaskService(MyDBContext context)
        {
            _context = context;
        }
        public async Task<List<TaskDTO>> GetAllTask()
        {
            //return await _context.Tasks.ToListAsync();
            return await _context.Tasks
                .Select(x => taskDTO(x))
                .ToListAsync();
        }
        public async Task<List<TaskDTO>> GetByStatus(bool status)
        {
            return await _context.Tasks.Where(x => x.Status == status).Select(x => taskDTO(x)).ToListAsync();
        }
        public async Task<List<TaskDTO>> GetByDate(DateTime date)
        {
            return await _context.Tasks.Where(x => x.CreateAt == date).Select(x => taskDTO(x)).ToListAsync();
        }
        public async Task<string> CompleteTasks(List<int> listIdTask)
        {
            //Duyệt listIDTask và lấy ra Task tương ứng
            int failId = 0; //Đếm số lần ID ko thoả mãn
            foreach (var idTask in listIdTask)
            {
                var Task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == idTask);
                //Kiểm tra Id có hợp lệ
                if (Task != null)
                {
                    Task.Status = true;
                    _context.Update(Task);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    failId++;
                }
            }
            if (failId == listIdTask.Count)
            {
                return "01";
            }
            else
            {
                return "00";
            }

        }
        public async Task<string> CreateTask(Models.Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return "00";
        }
        public async Task<string> DeleteTask(int Id)
        {
            var task = await _context.Tasks.FindAsync(Id);
            if (task == null)
            {
                return "01";
            }
            else
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return "00";
        }
        public async Task<string> UpdateTask(int Id, Models.Task task)
        {
            if (Id != task.Id)
            {
                return "01";
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
                    return "01";
                }
                else
                {
                    throw;
                }
            }

            return "00";
        }
        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
        private static TaskDTO taskDTO(Models.Task task) =>
           new TaskDTO
           {
               Name = task.Name,
               Description = task.Description,
               Status = task.Status,
               ExecAt = task.ExecAt,
               CreateAt = task.CreateAt
           };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.IService
{
    public interface ITaskServices
    {
        Task<List<TaskRequest>> GetAllTask(int userId);
        Task<List<TaskRequest>> GetByStatus(int userId, bool status);
        Task<List<TaskRequest>> GetByDate(int userId, DateTime date);
        Task<string> CompleteTasks(int userId, List<int> listIdTask);
        Task<Models.Task> CreateTask(int userId, Models.Task task);
        Task<string> DeleteTask(int userId, int Id);
        Task<Models.Task> UpdateTask(int userId, int Id, Models.Task task);
    }
}

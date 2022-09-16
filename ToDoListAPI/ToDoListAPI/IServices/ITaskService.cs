using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.IService
{
    public interface ITaskService
    {
        Task<List<Models.Task>> GetTasks(int userId, bool status, DateTime date);
        Task<string> CompleteTasks(int userId, List<int> listIdTask);
        Task<Models.Task> CreateTask(int userId, TaskRespon taskDTO);
        Task<string> DeleteTask(int userId, int Id);
        Task<Models.Task> UpdateTask(int userId, TaskRespon taskDTO);
    }
}

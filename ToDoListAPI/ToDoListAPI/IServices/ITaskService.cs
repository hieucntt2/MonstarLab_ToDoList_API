using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.IService
{
    public interface ITaskService
    {
        Task<List<TaskRequest>> GetTask(int userId, bool status, DateTime date);
        Task<string> CompleteTasks(int userId, List<int> listIdTask);
        Task<TaskRequest> CreateTask(int userId, TaskRequest taskDTO);
        Task<string> DeleteTask(int userId, int Id);
        Task<TaskRequest> UpdateTask(int userId, TaskRequest taskDTO);
    }
}

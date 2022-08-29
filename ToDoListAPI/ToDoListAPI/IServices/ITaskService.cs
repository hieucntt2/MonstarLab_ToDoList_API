using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.IService
{
    public interface ITaskService
    {
        Task<List<TaskDTO>> GetAllTask();
        Task<List<TaskDTO>> GetByStatus(bool status);
        Task<List<TaskDTO>> GetByDate(DateTime date);
        Task<string> CompleteTasks(List<int> listIdTask);
        Task<string> CreateTask(Models.Task task);
        Task<string> DeleteTask(int Id);
        Task<string> UpdateTask(int Id, Models.Task task);
    }
}

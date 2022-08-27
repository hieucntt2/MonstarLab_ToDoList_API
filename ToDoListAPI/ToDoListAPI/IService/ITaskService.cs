using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAPI.IService
{
    public interface ITaskService
    {
        Task<List<Models.Task>> GetAll();
        Task<List<Models.Task>> GetByStatus(bool status);
        Task<List<Models.Task>> GetByDate(DateTime date);
        Task<string> CompleteTasks(List<int> listIdTask);
        Task<string> CreateTask(Models.Task task);
        Task<string> DeleteTask(int Id);
        Task<string> UpdateTask(int Id, Models.Task task);
    }
}

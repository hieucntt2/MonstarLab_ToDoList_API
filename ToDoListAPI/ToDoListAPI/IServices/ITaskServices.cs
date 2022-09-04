﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.IService
{
    public interface ITaskServices
    {
        Task<List<TaskDTO>> GetAllTask(int userId);
        Task<List<TaskDTO>> GetByStatus(bool status);
        Task<List<TaskDTO>> GetByDate(DateTime date);
        Task<string> CompleteTasks(List<int> listIdTask);
        Task<Models.Task> CreateTask(Models.Task task);
        Task<string> DeleteTask(int Id);
        Task<string> UpdateTask(int Id, Models.Task task);
    }
}
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListAPI.Controllers;
using ToDoListAPI.IService;
using ToDoListAPI.Models;
using ToDoListAPI.Services;
using Xunit;

namespace CustomerApiMoq
{
    public class TestToDoList
    {
        public Mock<ITaskService> mock = new Mock<ITaskService>();
        [Fact]
        public void TestDeleteTask()
        {
            mock.Setup(x => x.DeleteTask(3, 11))
                .ReturnsAsync("Delete Sucess !");
            var taskService = new TasksController(mock.Object, null);
            var result = taskService.DeleteTask(11);
        }
        [Fact]
        public void CreateTask()
        {
            var taskDTO = new TaskRespon()
            {
                Id = 1,
                Name = "BTVN",
                Description = "BTVN",
                ExecAt = DateTime.Now,
                CateId = 1
            };
            mock.Setup(x => x.CreateTask(3, taskDTO));
            var taskService = new TasksController(mock.Object, null);
            var result = taskService.CreateTask(taskDTO);
        }
    }
}

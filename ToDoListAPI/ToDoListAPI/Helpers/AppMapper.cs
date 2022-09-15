using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.Helpers
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<TaskRespon, Models.Task>();
            CreateMap<UserRequest, User>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAPI.Models
{
    public class TaskRequest
    {
        [Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime ExecAt { get; set; }
        public DateTime CreateAt { get; set; }
        public int UserId { get; set; }
        public int CateId { get; set; }


    }
}

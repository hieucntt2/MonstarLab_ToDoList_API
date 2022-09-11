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
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public DateTime ExecAt { get; set; }
        [Required]
        public int CateId { get; set; }
    }
}
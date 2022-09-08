using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAPI.Models
{
    [Table("Task")]
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime ExecAt { get; set; }
        public DateTime CreateAt { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int CateId { get; set; }
        [ForeignKey("CateId")]
        public TaskCategory Category { get; set; }

        public void ConvertFormTaskDTO(TaskRequest task)
        {
            this.Name = task.Name;
            this.Description = task.Description;
            this.Status = task.Status;
            this.ExecAt = task.ExecAt;
            this.CreateAt = task.CreateAt;
            this.UserId = task.UserId;
            this.CateId = task.CateId;
        }
    }
}

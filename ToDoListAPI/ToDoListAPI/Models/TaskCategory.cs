using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAPI.Models
{
    [Table("TaskCategories")]
    public class TaskCategory
    {
        [Key]
        public int CateId { get; set; }
        [Required]
        [MaxLength(100)]
        public string CateName { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}

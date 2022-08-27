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
        [Display(Name = "Tên công việc")]
        public string Name { get; set; }
        [StringLength(1000)]
        [Display(Name = "Mô tả công việc")]
        public string Description { get; set; }
        [Display(Name = "Trạng thái ")]
        public bool Status { get; set; }
        [Display(Name = "Ngày hoàn thành")]
        public DateTime ExecAt { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        public DateTime CreateAt { get; set; }

    }
}

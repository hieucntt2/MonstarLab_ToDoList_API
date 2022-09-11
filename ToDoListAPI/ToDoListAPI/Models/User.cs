using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAPI.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FullName { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public ICollection<Task> Tasks { get; set; }
        public void ConvertFormUserDTO(UserRequest input)
        {
            this.FullName = input.FullName;
            this.UserName = input.UserName;
            this.Password = input.Password;
            this.Email = input.Email;
        }
        internal void ConvertFormTaskDTO(TaskRequest taskDTO)
        {
            throw new NotImplementedException();
        }
    }
}

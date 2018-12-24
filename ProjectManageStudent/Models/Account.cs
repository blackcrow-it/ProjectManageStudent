using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Models
{
    using System.Collections;

    public class Account 
    {
        public Account()
        {
            this.CreateAt = DateTime.Now;
            this.UpdateAt = DateTime.Now;
            this.Status = AccountStatus.Active;
        }
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public Role Role { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime BirthDay { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public AccountStatus Status { get; set; }
        public List<Mark> Marks { get; set; }
        [ForeignKey("ClassroomId")]
        public Classroom Classroom { get; set; }

        public bool checkRole()
        {
            return (this.Role == Role.Ministry);
        }
        public bool checkRoleST()
        {
            return (this.Role == Role.student);
        }

      
    }
    public enum AccountStatus
    {
        Active = 1,
        Deactive = 0
    }

    public enum Role
    {
        Ministry = 1,
        student = 0
    }
}

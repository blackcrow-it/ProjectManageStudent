using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Models
{
    using Newtonsoft.Json;
    using System.Collections;

    public class Account 
    {
        public Account()
        {
            this.CreateAt = DateTime.Now;
            this.UpdateAt = DateTime.Now;
            this.Status = AccountStatus.Active;
           
        }
        public Account(string newPassword)
        {
            this.Password = newPassword;
            this.UpdateAt = DateTime.Now;
        }
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Url]
        public string Avartar { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        
        public string FirstName { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }
        public genderStatus Gender { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        [JsonIgnore]
        public AccountStatus Status { get; set; }
        [JsonIgnore]
        public List<Mark> Marks { get; set; }
        [ForeignKey("ClassroomId")]
        [JsonIgnore]
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

    public enum genderStatus
    {
        Male = 1,
        Female = 2,
        Other = 0
    }
    public enum Role
    {
        Ministry = 1,
        student = 0
    }
}

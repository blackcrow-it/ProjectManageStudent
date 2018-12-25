using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Models
{
    public class ListStudentInClassroom
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int ClassroomId { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ForeignKey("ClassroomId")]
        public Classroom Classroom { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}

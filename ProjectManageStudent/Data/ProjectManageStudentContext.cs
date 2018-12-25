using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManageStudent.Models;

namespace ProjectManageStudent.Models
{
    public class ProjectManageStudentContext : DbContext
    {
        public ProjectManageStudentContext (DbContextOptions<ProjectManageStudentContext> options)
            : base(options)
        {
        }

        public DbSet<ProjectManageStudent.Models.Classroom> Classroom { get; set; }

        public DbSet<ProjectManageStudent.Models.Account> Account { get; set; }

        public DbSet<ProjectManageStudent.Models.Subject> Subject { get; set; }

        public DbSet<ProjectManageStudent.Models.Mark> Mark { get; set; }
        public DbSet<ProjectManageStudent.Models.Credential> Credential { get; set; }
        public DbSet<ProjectManageStudent.Models.ListStudentInClassroom> ListStudentInClassroom { get; set; }
    }
}

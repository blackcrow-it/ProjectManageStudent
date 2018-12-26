using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Models
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class Subject
    {
        public Subject()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdateAt = DateTime.Now;

        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<Mark> Marks { get; set; }

        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Models
{
    public class Mark
    {
        public Mark()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdateAt = DateTime.Now;
            this.Practice = -1 ;
            this.Theory = -1;
            this.Assignment = -1;
            this.Status = MarkStatus.Null;
            
        }
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SubjectId { get; set; }
        public float Theory { get; set; }
        public float Practice { get; set; }
        public float Assignment { get; set; }
        public MarkStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
    }
    public enum MarkStatus {
        Pass = 1,
        Fail = 0,
        Null = -1
    }
    
}

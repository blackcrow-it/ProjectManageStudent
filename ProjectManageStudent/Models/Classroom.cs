using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class Classroom 
    {
        public Classroom()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public List<Account> Accounts { get; set; }
     
    }
}

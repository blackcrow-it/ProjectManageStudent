using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Models
{
    public class ChangeInformation
    {
        public string Phone { get; set; }
        //public string Address { get; set; }
        //public string Avatar { get; set; }
        public string NewPhone { get; set; }
        //public string NewAddress { get; set; }
        //public string NewAvatar { get; set; }
    }
}

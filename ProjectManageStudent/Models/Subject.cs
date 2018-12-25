using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Models
{
    using System.Collections;

    public class Subject : IEnumerable
    {
        public Subject()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdateAt = DateTime.Now;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<Mark> Marks { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

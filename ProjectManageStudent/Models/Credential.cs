using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.Models
{
    public class Credential
    {
        public Credential(int OwnerId)
        {
            this.AccessToken = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Status = CredentialStatus.Active;
            this.ExpiredAt = DateTime.Now.AddDays(7);
            this.OwnerId = OwnerId;
        }
        [Key]
        public string AccessToken { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public CredentialStatus Status { get; set; }
        public bool isValid()
        {
            return (this.Status == CredentialStatus.Active && this.ExpiredAt > DateTime.Today);
        }
    }

    public enum CredentialStatus
    {
        Active = 1,
        Deactive = 0
    }
}

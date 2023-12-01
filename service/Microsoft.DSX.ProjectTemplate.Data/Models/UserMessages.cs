using System.Collections.Generic;

namespace Microsoft.DSX.ProjectTemplate.Data.Models
{
    public class UserMessages:BaseModel<int>
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        // Additional fields like profile picture, status, etc.

        // Navigation properties
        public ICollection<Message> Messages { get; set; }
    }

}
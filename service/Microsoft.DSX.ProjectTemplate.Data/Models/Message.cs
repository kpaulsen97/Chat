using System;

namespace Microsoft.DSX.ProjectTemplate.Data.Models
{
    public class Message:BaseModel<int>
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }


        // Foreign keys and navigation properties for relational integrity
        public int SenderId { get; set; }
        public UserMessages Sender { get; set; }
    }
}
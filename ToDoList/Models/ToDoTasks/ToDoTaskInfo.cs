using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    public class ToDoTaskInfo
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("UserId")]
        public Guid UserId { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("LastUpdatedAt")]
        public DateTime LastUpdatedAt { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("EndAt")]
        public DateTime EndAt { get; set; }

        [BsonElement("IsDone")]
        public bool IsDone { get; set; }

        [BsonElement("Priority")]
        public ToDoTaskPriority Priority { get; set; }
    }
}

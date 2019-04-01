using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    public class ToDoTask : ToDoTaskInfo
    {
        [BsonElement("Text")]
        public string Text { get; set; }
    }
}

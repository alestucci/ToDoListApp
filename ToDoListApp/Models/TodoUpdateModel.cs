using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Models
{
    public class TodoUpdateModel
    {
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }

    }
}

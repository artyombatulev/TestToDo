using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoBusinessLogic.DTO
{
    public class TodoDTO
    {
        public int TodoId { get; set; }
        public string Title { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool Completed { get; set; }

        public ICollection<TodoPointDTO> Points { get; set; }
    }
}

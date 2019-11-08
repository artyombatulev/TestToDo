using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoBusinessLogic.DTO
{
    public class TodoPointDTO
    {
        public int PointId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateOfComplition { get; set; }

        public int TodoId { get; set; } 
    }
}

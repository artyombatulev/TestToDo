using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class TodoPointViewModel
    {
        public int PointId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateOfComplition { get; set; }
        public int TodoId { get; set; }
    }
}

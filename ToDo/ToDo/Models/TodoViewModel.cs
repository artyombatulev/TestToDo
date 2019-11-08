using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class TodoViewModel
    {
        public int TodoId { get; set; }
        public string Title { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool Completed { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ToDo.ViewModels
{
    public class TodoViewModel
    {
        public int TodoId { get; set; }
        public string Title { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool Completed { get; set; }

        public ICollection<TodoPointViewModel> Points { get; set; }
    }
}

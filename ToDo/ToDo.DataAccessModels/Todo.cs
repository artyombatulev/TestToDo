using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.DataAccessModels.BaseModels;

namespace ToDo.DataAccessModels
{
    public class Todo : BaseEntity
    {
        public Todo()
        {
            Points = new HashSet<TodoPoint>();
        }

        [Key]
        public int TodoId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool Completed { get; set; } = false;
        
        public virtual ICollection<TodoPoint> Points { get; set; }
    }
}

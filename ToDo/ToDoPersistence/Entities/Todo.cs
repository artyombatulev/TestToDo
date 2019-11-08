using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ToDoPersistence.Entities
{
    public class Todo
    {
        [Key]
        public int TodoId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreationDate { get; set; }
        //[Required]
        public ICollection<TodoPoint> Point { get; set; }
        [Required]
        [Column(TypeName = "bit")]
        public bool Completed { get; set; } = false;
    }
}

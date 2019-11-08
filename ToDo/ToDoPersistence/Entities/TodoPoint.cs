using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ToDoPersistence.Entities
{
    public class TodoPoint
    {
        [Key]
        public int PointId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(300)")]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "bit")]
        public bool IsCompleted { get; set; } = false;
        public DateTime? DateOfComplition { get; set; } = null;
        [Required]
        public Todo Todo { get; set; }
        [Required]
        public int TodoId { get; set; }
    }
}

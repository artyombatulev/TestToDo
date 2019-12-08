using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.DataAccessModels.BaseModels;

namespace ToDo.DataAccessModels
{
    public class TodoPoint : BaseEntity
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
        public int TodoId { get; set; }
        
        public virtual Todo Todo { get; set; }
    }
}

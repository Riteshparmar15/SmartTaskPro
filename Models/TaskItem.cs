using System.ComponentModel.DataAnnotations;

namespace SmartTaskPro.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Title { get; set; }

        public string Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        // Assignment
        public int? AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }

        // Audit & soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SmartTaskPro.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        public Role Role { get; set; } = Role.User;

        public bool IsDeleted { get; set; } = false;

        // Audit
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }

        public ICollection<TaskItem> Tasks { get; set; }
    }
}

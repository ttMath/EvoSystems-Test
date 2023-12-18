using EvoSystems.Service.DepartamentService;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSystems.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string RG { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public string Picture { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToLocalTime();
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now.ToLocalTime();
    }
}

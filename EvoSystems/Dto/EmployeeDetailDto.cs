using System.ComponentModel.DataAnnotations;

namespace EvoSystems.Dto
{
    public class EmployeeDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RG { get; set; }
        public int DepartmentId { get; set; } = 0;
        public string Picture { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}

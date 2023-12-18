using System.ComponentModel.DataAnnotations;

namespace EvoSystems.Dto
{
    public class DepartmentDetailDto
    {
        public int Id { get; set; }       
        public string Name { get; set; }        
        public string Abbreviation { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Position:BaseEntity
    {
        [MinLength(3,ErrorMessage ="3den  az ola bilmez")]
        [MaxLength(15)]
        [Required]
        public string Name { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}

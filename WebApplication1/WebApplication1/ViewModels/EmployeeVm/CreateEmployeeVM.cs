using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class CreateEmployeeVM
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        public string Surname { get; set; }
        [Required]
        public IFormFile? Photo { get; set; }
        [Required]
        public string Description { get; set; }
        public string XUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedinUrl { get; set; }
        //relations
        [Required]
        public int? PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}

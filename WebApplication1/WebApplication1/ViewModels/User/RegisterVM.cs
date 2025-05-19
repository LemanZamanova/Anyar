using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class RegisterVM
    {
        [MinLength(3)]
        public string Name { get; set; }
        [MinLength(5)]
        public string Surname { get; set; }
        [MinLength(3)]
        public string UserName { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}


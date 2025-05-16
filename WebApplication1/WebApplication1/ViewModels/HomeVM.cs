using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class HomeVM
    {
        public List<Employee> Employees { get; set; }
        public DbSet<Employee> Employee { get; internal set; }
    }
}

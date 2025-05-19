using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;
using WebApplication1.Utilities;
using WebApplication1.Utilities.Extensions;
using WebApplication1.ViewModels;



namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }



        public async Task<IActionResult> Index()
        {
            List<GetEmployeeVM> employeeVM = await _context.Employees.Include(e => e.Positions).Select(e => new GetEmployeeVM
            {
                PositionName = e.Positions.Name,

                Id = e.Id,
                Name = e.Name,
                Surname = e.Surname,

                Image = e.Image


            }
            ).ToListAsync();


            return View(employeeVM);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM employeeVM = new CreateEmployeeVM
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            employeeVM.Positions = await _context.Positions.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(employeeVM);
            }
            if (!employeeVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "sekilin tipi sehvdir");
                return View(employeeVM);
            }

            if (!employeeVM.Photo.ValidateSize(Utilities.FileSize.MB, 5))
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "sekilin olcusu 5MB dan coxdur");
                return View(employeeVM);
            }
            string fileName = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "team");
            Employee employee = new Employee
            {
                Name = employeeVM.Name,
                Image = fileName,
                PositionId = employeeVM.PositionId.Value,
                Description = employeeVM.Description,
                Surname = employeeVM.Surname,
                XUrl = employeeVM.XUrl,
                InstagramUrl = employeeVM.InstagramUrl,
                FacebookUrl = employeeVM.FacebookUrl,
                LinkedinUrl = employeeVM.LinkedinUrl,
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {

            if (id is null || id < 1) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee is null) return BadRequest();
            UpdateEmployeeVM employeeVM = new UpdateEmployeeVM
            {
                Description = employee.Description,
                XUrl = employee.XUrl,
                InstagramUrl = employee.InstagramUrl,
                FacebookUrl = employee.FacebookUrl,
                LinkedinUrl = employee.LinkedinUrl,
                Name = employee.Name,
                Surname = employee.Surname,


                Image = employee.Image,
                PositionId = employee.PositionId,
                Positions = _context.Positions.ToList(),

            };
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateEmployeeVM employeeVM)
        {
            employeeVM.Positions = await _context.Positions.ToListAsync();

            if (!ModelState.IsValid) return View(employeeVM);

            if (employeeVM.Image != null)
            {
                if (!employeeVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVM.Image), "File Type is incorrect");
                    return View(employeeVM);
                }
                if (!employeeVM.Photo.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVM.Image), "File size can't be greater than 2MB");
                    return View(employeeVM);
                }
            }

            bool positionExists = _context.Positions.Any(p => p.Id == employeeVM.PositionId);
            if (!positionExists)
            {
                ModelState.AddModelError(nameof(UpdateEmployeeVM.PositionId), "Position doesn't exist");
                return View(employeeVM);
            }

            bool nameExists = await _context.Employees.AnyAsync(e => e.Name == employeeVM.Name && e.Id != id);
            if (nameExists)
            {
                ModelState.AddModelError(nameof(UpdateEmployeeVM.Name), "Employee with this name already exists");
                return View(employeeVM);
            }

            Employee? existed = await _context.Employees
                .Include(e => e.Positions)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existed is null) return NotFound();


            string fileName = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "team");
            existed.Image = fileName;
            existed.Name = employeeVM.Name;
            existed.Surname = employeeVM.Surname;
            existed.Description = employeeVM.Description;
            existed.PositionId = employeeVM.PositionId;
            existed.XUrl = employeeVM.XUrl;
            existed.InstagramUrl = employeeVM.InstagramUrl;
            existed.FacebookUrl = employeeVM.FacebookUrl;

            existed.LinkedinUrl = employeeVM.LinkedinUrl;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)

        {
            if (id is null || id <= 0) return BadRequest();

            Employee? employee = await _context.Employees

               .FirstOrDefaultAsync(x => x.Id == id);

            if (employee is null)
            {
                return NotFound();
            }

            employee.Image.DeleteFile(_env.WebRootPath, "assets", "img", "team");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




    }




}


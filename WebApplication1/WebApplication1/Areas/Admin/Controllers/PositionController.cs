using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Position>positions= await _context.Positions.Include(p=>p.Employees).AsNoTracking().ToListAsync();
            return View(positions);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create( Position position)
        {
            if(!ModelState.IsValid) 
                {
                return View(position);
                }
            bool result=await _context.Positions.AnyAsync(p=>p.Name == position.Name);
            if(result) 
                {
                ModelState.AddModelError(nameof(Position.Name),$"{position.Name} named alredy exists");
               return View();
            }
           await _context.Positions.AddAsync(position);
             await _context.SaveChangesAsync();
           return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {

            if (id is null || id <= 0) return BadRequest();
           Position? position = await _context.Positions.FirstOrDefaultAsync(p=>p.Id == id);
            if(position is null) return NotFound();
            return View(position);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,Position position)
        {
            if (!ModelState.IsValid)
            {
                return View();


            }
            bool result= await _context.Positions.AnyAsync(p=>p.Name == position.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(Position.Name), $"{position.Name} already exists");

                return View();
            }
            Position? existed = await _context.Positions.FirstOrDefaultAsync(c=>c.Id==id);
            existed.Name = position.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) { return BadRequest(); }
            Position? existed = await _context.Positions.FirstOrDefaultAsync(c => c.Id == id);

            if (existed is null) return NotFound();

           
            _context.Positions.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }


    }
    }

using VeterinaryPassport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VeterinaryPassport.Controllers
{
    [Authorize(Roles = "user")]
    public class PetController : Controller
    {
        Context db;
        public PetController(Context context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> PetRead(string currentFilter, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var pet = db.Pets.Select(p => p);
            int pageSize = 5;

            return View(await Pagination<Pet>.CreatePaginationAsync(pet, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult PetCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PetCreate(Pet pet)
        {
            if (ModelState.IsValid)
            {
                await db.Pets.AddAsync(pet);
                await db.SaveChangesAsync();

                return RedirectToAction("PetRead");
            }
            return View(pet);
        }

        [HttpGet]
        public async Task<IActionResult> PetEdit(int? id)
        {
            if (id != null)
            {
                Pet pet = await db.Pets.FirstOrDefaultAsync(p => p.Id == id);
                if (pet != null)
                    return View(pet);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PetEdit(Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Pets.Update(pet);
                await db.SaveChangesAsync();

                return RedirectToAction("PetRead");
            }
            return View(pet);
        }

        [HttpPost]
        public async Task<IActionResult> PetDelete(int? id)
        {
            if (id != null)
            {
                Pet pet = await db.Pets.FirstOrDefaultAsync(p => p.Id == id);
                db.Pets.Remove(pet);
                await db.SaveChangesAsync();

                return RedirectToAction("PetRead");
            }
            return NotFound();
        }
    }
}

using VeterinaryPassport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VeterinaryPassport.Controllers
{
    public class VetController : Controller
    {
        Context db;
        public VetController(Context context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> VetRead(string currentFilter, string searchString, int? pageNumber)
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
            var vet = db.Vets.Select(v => v);
            int pageSize = 5;

            return View(await Pagination<Vet>.CreatePaginationAsync(vet, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult VetCreate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VetCreate(Vet vet)
        {
            if (ModelState.IsValid)
            {
                await db.Vets.AddAsync(vet);
                await db.SaveChangesAsync();

                return RedirectToAction("VetRead");
            }
            return View(vet);
        }

        [HttpGet]
        public async Task<IActionResult> VetEdit(int? id)
        {
            if (id != null)
            {
                Vet vet = await db.Vets.FirstOrDefaultAsync(v => v.Id == id);
                if (vet != null)
                    return View(vet);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> VetEdit(Vet vet)
        {
            if (ModelState.IsValid)
            {
                db.Vets.Update(vet);
                await db.SaveChangesAsync();

                return RedirectToAction("VetRead");
            }
            return View(vet);
        }

        [HttpPost]
        public async Task<IActionResult> VetDelete(int? id)
        {
            if (id != null)
            {
                Vet vet = await db.Vets.FirstOrDefaultAsync(v => v.Id == id);
                db.Vets.Remove(vet);
                await db.SaveChangesAsync();

                return RedirectToAction("VetRead");
            }
            return NotFound();
        }
    }
}

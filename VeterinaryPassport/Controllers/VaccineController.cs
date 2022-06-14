using VeterinaryPassport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VeterinaryPassport.Controllers
{
    public class VaccineController : Controller
    {
        Context db;
        public VaccineController(Context context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> VaccineRead(int? id, string currentFilter, string searchString, int? pageNumber)
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
           

            var vaccine = db.Vaccines.Where(v => v.PassportId == id).Include(vet => vet.Vet)
                .Include(p =>p.Passport).ThenInclude(pet => pet.Pet).Select(va => va);

            //Pet pet = await db.Pets.FirstOrDefaultAsync(p => p.Id == id);
            //pet.Passport = await db.Passports.FirstOrDefaultAsync(p => p.Id == id);
            //ViewBag.pet = pet;
            //ViewBag.owner = await db.Owners.FirstOrDefaultAsync(o => o.Id == pet.Passport.Id);
            //passport.Owner = await db.Owners.FirstOrDefaultAsync(o => o.Id == passport.OwnerId);
            //passport.Pet = await db.Pets.FirstOrDefaultAsync(p => p.Id == passport.PetId);
            int pageSize = 5;

            return View(await Pagination<Vaccine>.CreatePaginationAsync(vaccine, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult VaccineCreate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VaccineCreate(Vaccine vaccine)
        {
            if (ModelState.IsValid)
            {
                await db.Vaccines.AddAsync(vaccine);
                await db.SaveChangesAsync();

                return RedirectToAction("VaccineRead");
            }
            return View(vaccine);
        }

        [HttpGet]
        public async Task<IActionResult> VaccineEdit(int? id)
        {
            if (id != null)
            {
                Vaccine vaccine = await db.Vaccines.FirstOrDefaultAsync(v => v.Id == id);
                if (vaccine != null)
                    return View(vaccine);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> VaccineEdit(Vaccine vaccine)
        {
            if (ModelState.IsValid)
            {
                db.Vaccines.Update(vaccine);
                await db.SaveChangesAsync();

                return RedirectToAction("VaccineRead");
            }
            return View(vaccine);
        }

        [HttpPost]
        public async Task<IActionResult> VaccineDelete(int? id)
        {
            if (id != null)
            {
                Vaccine vaccine = await db.Vaccines.FirstOrDefaultAsync(v => v.Id == id);
                db.Vaccines.Remove(vaccine);
                await db.SaveChangesAsync();

                return RedirectToAction("VaccineRead");
            }
            return NotFound();
        }
    }
}

using VeterinaryPassport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace VeterinaryPassport.Controllers
{
    [Authorize(Roles = "user")]
    public class VaccineController : Controller
    {
        Context db;
        public VaccineController(Context context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> VaccineRead(int? id, string currentFilter, string searchString, int? pageNumber, string Vaccines = null)
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

            var vaccine = db.Vaccines.Where(v => v.PassportId == id)
                .Include(vet => vet.Vet)
                .Include(p => p.Passport).ThenInclude(pet => pet.Pet)
                .Select(va => va);
            Passport passport = await db.Passports.FirstOrDefaultAsync(pas => pas.Id == id);
            Pet pet = await db.Pets.FirstOrDefaultAsync(p => p.Id == passport.PetId);
            Owner owner = await db.Owners.FirstOrDefaultAsync(o => o.Id == passport.OwnerId);
            ViewBag.pet = pet;
            ViewBag.owner = owner;
            ViewBag.passportId = passport.Id;

            var CountryLst = new List<string>();
            CountryLst.AddRange(vaccine.Select(v => v.Vet.Surname).Distinct());
            ViewData["Vaccines"] = new SelectList(CountryLst);

            if (!String.IsNullOrEmpty(searchString))
                vaccine = vaccine.Where(s => s.Name.Contains(searchString));

            if (!String.IsNullOrEmpty(Vaccines))
                vaccine = vaccine.Where(s => s.Vet.Surname.Contains(Vaccines));

            int pageSize = 5;

            return View(await Pagination<Vaccine>.CreatePaginationAsync(vaccine, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> VaccineCreate(int? id)
        {
            ViewBag.passportId = id;
            ViewData["VetList"] = new SelectList(db.Vets, "Id", "Surname");
            ViewData["Vet"] = await db.Vets.FirstOrDefaultAsync(v => v.Login == User.Identity.Name);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VaccineCreate(Vaccine vaccine)
        {
            if (ModelState.IsValid)
            {
                await db.Vaccines.AddAsync(vaccine);
                await db.SaveChangesAsync();

                return RedirectToAction("VaccineRead", "Vaccine", new { id = vaccine.PassportId });
            }
            else
            {
                ViewBag.passportId = vaccine.PassportId;
                ViewData["Vet"] = await db.Vets.FirstOrDefaultAsync(v => v.Login == User.Identity.Name);

                return View(vaccine);
            }
        }

        [HttpGet]
        public async Task<IActionResult> VaccineEdit(int? id)
        {
            if (id != null)
            {
                Vaccine vaccine = await db.Vaccines.FirstOrDefaultAsync(v => v.Id == id);
                Vet vet = await db.Vets.FirstOrDefaultAsync(v => v.Login == User.Identity.Name);
                bool isCorrectVet = vet.Id == vaccine.VetId;
                if (!isCorrectVet)
                {
                    return RedirectToAction($"VaccineRead", "Vaccine", new { id = vaccine.PassportId });
                }
                ViewBag.passportId = vaccine.PassportId;
                ViewData["VetList"] = new SelectList(db.Vets, "Id", "Surname");
                ViewData["Vet"] = await db.Vets.FirstOrDefaultAsync(v => v.Login == User.Identity.Name);
                if (vaccine != null)
                {
                    return View(vaccine);
                }
                    
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

                return RedirectToAction("VaccineRead", "Vaccine", new { id = vaccine.PassportId });
            }
            ViewBag.passportId = vaccine.PassportId;
            ViewData["VetList"] = new SelectList(db.Vets, "Id", "Surname");
            ViewData["Vet"] = await db.Vets.FirstOrDefaultAsync(v => v.Login == User.Identity.Name);

            return View(vaccine);
        }

        [HttpPost]
        public async Task<IActionResult> VaccineDelete(int? id)
        {
            if (id != null)
            {
                Vaccine vaccine = await db.Vaccines.FirstOrDefaultAsync(v => v.Id == id);
                Vet vet = await db.Vets.FirstOrDefaultAsync(v => v.Login == User.Identity.Name);
                bool isCorrectVet = vet.Id == vaccine.VetId;
                if (!isCorrectVet)
                {
                    return RedirectToAction($"VaccineRead", "Vaccine", new { id = vaccine.PassportId });
                }
                db.Vaccines.Remove(vaccine);
                await db.SaveChangesAsync();

                return RedirectToAction("VaccineRead", "Vaccine", new { id = vaccine.PassportId });
            }
            return NotFound();
        }
    }
}

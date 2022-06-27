using VeterinaryPassport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace VeterinaryPassport.Controllers
{
    [Authorize(Roles = "user")]
    public class PassportController : Controller
    {
        Context db;
        public PassportController(Context context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PassportRead(string currentFilter, string searchString, int? pageNumber)
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
            var passport = db.Passports.Include(o => o.Owner).Include(p => p.Pet).Select(p => p);
            if (!String.IsNullOrEmpty(searchString))
                passport = passport.Where(s => s.Owner.Surname.Contains(searchString));

            int pageSize = 5;

            return View(await Pagination<Passport>.CreatePaginationAsync(passport, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult PassportCreate()
        {
            ViewData["VaccinesListId"] = new SelectList(db.Owners, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PassportCreate(Pet pet, int id)
        {

            if (ModelState.IsValid)
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file != null)
                {
                    using (var stream = new FileStream(@"wwwroot\img\" + file.FileName, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    pet.ImageName = $"{file.FileName}";
                }
                else
                {
                    pet.ImageName = "Нет_фото.png";
                }
                return RedirectToAction("PassportCreateNext", pet);
            }
            return View(pet);
        }

        [HttpGet]
        public async Task<IActionResult> PassportCreateNext(Pet pet, string currentFilter, string searchString, int? pageNumber)
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
            var owner = db.Owners.Select(o => o);
            if (!String.IsNullOrEmpty(searchString))
                owner = owner.Where(s => s.Surname.Contains(searchString));
            int pageSize = 5;
            ViewBag.pet = pet;

            return View(await Pagination<Owner>.CreatePaginationAsync(owner, pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> PassportCreateNext(string name, string breed, string kind, DateTime dob, string sex, int ownerId,string imageName)
        {
            Pet pet = new()
            {
                Name = name,
                Breed = breed,
                DOB = dob,
                Kind = kind,
                Sex = sex,
                ImageName = imageName
            };

            await db.Pets.AddAsync(pet);
            await db.SaveChangesAsync();

            Passport passport = new()
            {
                PetId = pet.Id,
                OwnerId = ownerId
            };

            await db.Passports.AddAsync(passport);
            await db.SaveChangesAsync();

            return RedirectToAction("PassportRead");
        }

        [HttpGet]
        public async Task<IActionResult> PassportDetail(int? id)
        {
            Passport passport = await db.Passports.FirstOrDefaultAsync(p => p.Id == id);
            passport.Owner = await db.Owners.FirstOrDefaultAsync(o => o.Id == passport.OwnerId);
            passport.Pet = await db.Pets.FirstOrDefaultAsync(p => p.Id == passport.PetId);

            return View(passport);
        }

        [HttpPost]
        public async Task<IActionResult> PassportDelete(int? id)
        {
            if (id != null)
            {
                Passport passport = await db.Passports.FirstOrDefaultAsync(p => p.Id == id);
                Pet pet = await db.Pets.FirstOrDefaultAsync(p => p.Id == passport.PetId);
                db.Passports.Remove(passport);
                db.Pets.Remove(pet);
                await db.SaveChangesAsync();

                return RedirectToAction("PassportRead");
            }
            return NotFound();
        }
    }
}

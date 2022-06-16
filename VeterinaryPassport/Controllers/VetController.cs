using VeterinaryPassport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VeterinaryPassport.Controllers
{
    [Authorize(Roles = "admin")]
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
            if (!String.IsNullOrEmpty(searchString))
                vet = vet.Where(s => s.Surname.Contains(searchString));

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

                User user = new()
                {
                    Vet = vet,
                    Login = vet.Login,
                    Password = vet.Password,
                    VetId = vet.Id,
                    RoleId = 2
                };

                await db.Users.AddAsync(user);

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
                var user = await db.Users.FirstOrDefaultAsync(u => u.VetId == vet.Id);
                user.Login = vet.Login;
                user.Password = vet.Password;
                db.Users.Update(user);
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

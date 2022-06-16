using VeterinaryPassport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VeterinaryPassport.Controllers
{
    [Authorize(Roles = "user")]
    public class OwnerController : Controller
    {
        Context db;
        public OwnerController(Context context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> OwnerRead(string currentFilter, string searchString, int? pageNumber)
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

            return View(await Pagination<Owner>.CreatePaginationAsync(owner, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult OwnerCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OwnerCreate(Owner owner)
        {
            if (ModelState.IsValid)
            {
                await db.Owners.AddAsync(owner);
                await db.SaveChangesAsync();

                return RedirectToAction("OwnerRead");
            }
            return View(owner);
        }

        [HttpGet]
        public async Task<IActionResult> OwnerEdit(int? id)
        {
            if (id != null)
            {
                Owner owner = await db.Owners.FirstOrDefaultAsync(o => o.Id == id);
                if (owner != null)
                    return View(owner);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> OwnerEdit(Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.Owners.Update(owner);
                await db.SaveChangesAsync();

                return RedirectToAction("OwnerRead");
            }
            return View(owner);
        }

        [HttpPost]
        public async Task<IActionResult> OwnerDelete(int? id)
        {
            if (id != null)
            {
                Owner owner = await db.Owners.FirstOrDefaultAsync(o => o.Id == id);
                db.Owners.Remove(owner);
                await db.SaveChangesAsync();

                return RedirectToAction("OwnerRead");
            }
            return NotFound();
        }
    }
}

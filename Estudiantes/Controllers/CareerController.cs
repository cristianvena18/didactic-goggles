using Estudiantes.Models;
using Estudiantes.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.Controllers
{
    [Authorize(Roles = Roles.SuperAdminRole)]
    public class CareerController : Controller
    {
        private readonly DatabaseContext databaseContext;

        public CareerController(DatabaseContext context)
        {
            this.databaseContext = context;
        }

        // GET: Careers
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            Paginator paginator = new Paginator
            {
                CurrentPage = page,
                ItemsByPage = 5,
                TotalItems = await this.databaseContext.Careers.CountAsync()
            };
            ViewBag.Paginador = paginator;

            var items = this.databaseContext.Careers
                        .Skip((page - 1) * paginator.ItemsByPage)
                        .Take(paginator.ItemsByPage);

            return View(await items.ToListAsync());
        }

        // GET: Careers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Career = await this.databaseContext.Careers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Career == null)
            {
                return NotFound();
            }

            return View(Career);
        }

        // GET: Careers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Careers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] Career Career)
        {
            if (ModelState.IsValid)
            {
                this.databaseContext.Add(Career);
                await this.databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Career);
        }

        // GET: Careers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Career = await this.databaseContext.Careers.FindAsync(id);
            if (Career == null)
            {
                return NotFound();
            }
            return View(Career);
        }

        // POST: Careers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] Career Career)
        {
            if (id != Career.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.databaseContext.Update(Career);
                    await this.databaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CareerExists(Career.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Career);
        }

        // GET: Careers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Career = await this.databaseContext.Careers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Career == null)
            {
                return NotFound();
            }

            return View(Career);
        }

        // POST: Careers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Career = await this.databaseContext.Careers.FindAsync(id);
            this.databaseContext.Careers.Remove(Career);
            try
            {
                await this.databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("REFERENCE constraint"))
                    ModelState.AddModelError(string.Empty, "Ocurrio un error de integridad. Existen datos relacionados a este registro.");
                else
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(Career);
        }
        private bool CareerExists(int id)
        {
            return this.databaseContext.Careers.Any(e => e.Id == id);
        }
    }
}

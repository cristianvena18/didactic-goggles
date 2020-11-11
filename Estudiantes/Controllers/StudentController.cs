using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Estudiantes.Models;
using Estudiantes.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Estudiantes.Controllers
{
    [Authorize(Roles = Roles.AdminRole)]
    public class StudentController : Controller
    {
        private readonly DatabaseContext databaseContext;
        private readonly IWebHostEnvironment env;


        public StudentController(DatabaseContext context, IWebHostEnvironment env)
        {
            this.databaseContext = context;
            this.env = env;
        }

        // GET: Estudiantes
        [AllowAnonymous]
        public async Task<IActionResult> Index(string search, int? CareerId, int page = 1)
        {
            ShowRequest();
            int ItemsByPage = 4;

            //Generar consulta con los filtros
            var applicationDbContext = this.databaseContext.Students.Include(e => e.Career).Select(e => e);
            if (!string.IsNullOrEmpty(search))
            {
                applicationDbContext = applicationDbContext.Where(e => e.Name.Contains(search));
            }
            if (CareerId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(e => e.Career.Id == CareerId.Value);
            }

            //Generar pagina
            var registrosMostrar = applicationDbContext
                        .Skip((page - 1) * ItemsByPage)
                        .Take(ItemsByPage);


            //Create a view model to render
            StudentViewModel model = new StudentViewModel()
            {
                Students = await registrosMostrar.ToListAsync(),
                ListCareers = new SelectList(this.databaseContext.Careers, "Id", "Description", CareerId),
                search = search,
                CareerId = CareerId
            };

            model.Paginator.CurrentPage = page;
            model.Paginator.ItemsByPage = ItemsByPage;
            model.Paginator.TotalItems = await applicationDbContext.CountAsync();
            if (!string.IsNullOrEmpty(search))
                model.Paginator.QueryString.Add("search", search);
            if (CareerId.HasValue)
                model.Paginator.QueryString.Add("CareerId", CareerId.Value.ToString());


            return View(model);

        }

        private void ShowRequest()
        {
            Console.WriteLine("Query");

            Console.WriteLine(this.Request.QueryString);
            foreach (var item in Request.Query)
            {
                Console.WriteLine($"   {item.Key}: {item.Value}");
            }

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await this.databaseContext.Students
                .Include(e => e.Career)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            ViewData["CareerId"] = new SelectList(this.databaseContext.Careers, "Id", "Description");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Year,CareerId")] Student student)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    var photoFile = files[0];
                    var path = Path.Combine(env.WebRootPath, "images\\students");
                    if (photoFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(photoFile.FileName);

                        using (var filestream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            photoFile.CopyTo(filestream);
                            student.Picture = fileName;
                        };

                    }
                }


                this.databaseContext.Add(student);
                await this.databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CareerId"] = new SelectList(this.databaseContext.Careers, "Id", "Description", student.CareerId);
            return View(student);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var estudiante = await _context.Estudiantes.FindAsync(id);
            var student = await this.databaseContext.Students.Include(x => x.StudentSubjects).ThenInclude(m => m.Subject).FirstOrDefaultAsync(e => e.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["CareerId"] = new SelectList(this.databaseContext.Careers, "Id", "Description", student.CareerId);
            return View(student);
        }

        // POST: Estudiantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Year,CareerId,Picture")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    var photoFile = files[0];
                    var path = Path.Combine(env.WebRootPath, "images\\students");
                    if (photoFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(photoFile.FileName);

                        using (var filestream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            photoFile.CopyTo(filestream);

                            string oldFile = Path.Combine(path, student.Picture ?? "");
                            if (System.IO.File.Exists(oldFile))
                                System.IO.File.Delete(oldFile);
                            student.Picture = fileName;
                        };

                    }
                }


                try
                {
                    this.databaseContext.Update(student);
                    await this.databaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["CareerId"] = new SelectList(this.databaseContext.Careers, "Id", "Description", student.CareerId);
            return View(student);
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await this.databaseContext.Students
                .Include(e => e.Career)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await this.databaseContext.Students.FindAsync(id);
            this.databaseContext.Students.Remove(student);
            await this.databaseContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return this.databaseContext.Students.Any(e => e.Id == id);
        }
    }
}

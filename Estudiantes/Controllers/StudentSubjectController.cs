using Estudiantes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Students.Controllers
{
    public class StudentSubjectController : Controller
    {
        private readonly DatabaseContext databaseContext;

        public StudentSubjectController(DatabaseContext context)
        {
            this.databaseContext = context;
        }

        // GET: StudentSubjects
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = this.databaseContext.StudentSubjects.Include(e => e.Student).Include(e => e.Subject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StudentSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var StudentSubject = await this.databaseContext.StudentSubjects
                .Include(e => e.Student)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (StudentSubject == null)
            {
                return NotFound();
            }

            return View(StudentSubject);
        }

        // GET: StudentSubjects/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(this.databaseContext.Students, "Id", "Nombre");
            ViewData["SubjectId"] = new SelectList(this.databaseContext.Subjects, "Id", "Nombre");
            return View();
        }

        // POST: StudentSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,SubjectId,DateInscription,Year")] StudentSubject StudentSubject)
        {
            if (ModelState.IsValid)
            {
                this.databaseContext.Add(StudentSubject);
                await this.databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(this.databaseContext.Students, "Id", "Nombre", StudentSubject.StudentId);
            ViewData["SubjectId"] = new SelectList(this.databaseContext.Subjects, "Id", "Nombre", StudentSubject.SubjectId);
            return View(StudentSubject);
        }

        // GET: StudentSubjects/Edit/5
        // public async Task<IActionResult> Edit(string idCompuesto)

        //    [HttpGet("{idStudent}/{idSubject}")]
        public async Task<IActionResult> Edit(int? idStudent, int? idSubject)
        //public async Task<IActionResult> Edit([Bind("StudentId,SubjectId,FechaInscripcion")] StudentSubject StudentSubject2)

        {
            if (idStudent == null || idSubject == null)
            {
                return NotFound();
            }

            //var StudentSubject = await this.databaseContext.StudentSubjects.FindAsync(StudentSubject2.StudentId, StudentSubject2.SubjectId);
            var StudentSubject = await this.databaseContext.StudentSubjects.FindAsync(idStudent, idSubject);
            if (StudentSubject == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(this.databaseContext.Students, "Id", "Name", StudentSubject.StudentId);
            ViewData["SubjectId"] = new SelectList(this.databaseContext.Subjects, "Id", "Name", StudentSubject.SubjectId);
            return View(StudentSubject);
        }

        // POST: StudentSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("StudentId,SubjectId,DateInscription,Year")] StudentSubject StudentSubject)
        {
            //if (id != StudentSubject.StudentId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    this.databaseContext.Update(StudentSubject);
                    await this.databaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentSubjectExists(StudentSubject.StudentId))
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
            ViewData["StudentId"] = new SelectList(this.databaseContext.Students, "Id", "Name", StudentSubject.StudentId);
            ViewData["SubjectId"] = new SelectList(this.databaseContext.Subjects, "Id", "Name", StudentSubject.SubjectId);
            return View(StudentSubject);
        }

        // GET: StudentSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var StudentSubject = await this.databaseContext.StudentSubjects
                .Include(e => e.Student)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (StudentSubject == null)
            {
                return NotFound();
            }

            return View(StudentSubject);
        }

        // POST: StudentSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentSubject = await this.databaseContext.StudentSubjects.FindAsync(id);
            this.databaseContext.StudentSubjects.Remove(studentSubject);
            await this.databaseContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentSubjectExists(int id)
        {
            return this.databaseContext.StudentSubjects.Any(e => e.StudentId == id);
        }
    }
}

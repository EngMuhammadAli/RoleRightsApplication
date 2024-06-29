using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication10.DbContext;
using WebApplication10.Models.NewFolder;
using System.Linq;

namespace WebApplication10.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public StudentController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

      
        public IActionResult Index()
        {
            var students = _applicationDbContext.student.ToList();
            return View(students);
        }

        // GET: /Student/Create
        [RoleBasedAuthorization("Admin")] // Only Admin can access create view
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        [HttpPost]
        [RoleBasedAuthorization("Admin")] // Custom action filter for create action
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.student.Add(student);
                _applicationDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: /Student/Edit/{id}
        //[HttpGet("{id}")]
        //[RoleBasedAuthorization("Admin", "Teacher", "Student")] // Custom action filter for edit action
        //public IActionResult Edit(int id)
        //{
        //    var student = GetStudentById(id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(student);
        //}

        //// POST: /Student/Edit/{id}
        //[HttpPost("{id}")]
        //[RoleBasedAuthorization("Admin", "Teacher")] // Custom action filter for edit action
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(int id, StudentModel student)
        //{
        //    if (id != student.Id)
        //    {
        //        return BadRequest();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _applicationDbContext.student.Update(student);
        //        _applicationDbContext.SaveChanges();
        //        return View(student);
        //    }

        //    return View(student);
        //}

        //// GET: /Student/Delete/{id}
        //[HttpGet("{id}")]
        //[RoleBasedAuthorization("Admin")] // Custom action filter for delete action
        //public IActionResult Delete(int id)
        //{
        //    var student = GetStudentById(id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(student);
        //}

        //// POST: /Student/Delete/{id}
        //[Authorize(Policy = "AdminPolicy")] // Only Admin can delete student
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var student = GetStudentById(id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }

        //    _applicationDbContext.student.Remove(student);
        //    _applicationDbContext.SaveChanges();
        //    return View(student);
        //}

        //// Helper method to get student by ID
        //private StudentModel GetStudentById(int id)
        //{
        //    var student = _applicationDbContext.student.FirstOrDefault(s => s.Id == id);
        //    return student;
        //}
    }
}

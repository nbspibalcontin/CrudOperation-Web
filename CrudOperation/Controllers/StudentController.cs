using CrudOperation.Data;
using CrudOperation.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudOperation.Controllers
{
    public class StudentController : Controller
    {
        private readonly MvcDbContext mvcDbContext;

        public StudentController(MvcDbContext mvcDbContext)
        {
            this.mvcDbContext = mvcDbContext;
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel addStudentViewModelRequest)
        {
            var Student = new Student()
            {
                Id = Guid.NewGuid(),
                Name = addStudentViewModelRequest.Name,
                Email = addStudentViewModelRequest.Email,
            };

            await mvcDbContext.Students.AddAsync(Student);
            await mvcDbContext.SaveChangesAsync();
            return RedirectToAction("AddStudent");
        }
    }
}

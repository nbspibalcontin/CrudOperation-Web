using CrudOperation.Data;
using CrudOperation.Models;
using CrudOperation.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudOperation.Controllers
{
    public class StudentController : Controller
    {
        private readonly MvcDbContext mvcDbContext;

        public StudentController(MvcDbContext mvcDbContext)
        {
            this.mvcDbContext = mvcDbContext;
        }

        // Display all student data in Table
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await mvcDbContext.Students.ToListAsync();

            return View(students);
        }

        // View student form to create student
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        // Create Student 
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
            return RedirectToAction("Index");
        }
        
        // View the student by Id
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var student = mvcDbContext.Students.SingleOrDefault(x => x.Id == id);

            if (student != null) {

                var viewModel = new UpdateStudentViewModel()
                {
                    Id = id,
                    Name = student.Name,
                    Email = student.Email,
                };

                return await Task.Run(() => View("View",viewModel));          
            }
            return RedirectToAction("Index");
        }

        //Update student data
        [HttpPost]
        public async Task<IActionResult> View(UpdateStudentViewModel model)
        {
            var student = await mvcDbContext.Students.FindAsync(model.Id);

            if (student != null)
            {
                student.Name = model.Name;
                student.Email = model.Email;             

                await mvcDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //Delete Student data
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateStudentViewModel model)
        {
            var student = await mvcDbContext.Students.FindAsync(model.Id);

            if (student != null)
            {
                mvcDbContext.Students.Remove(student);
                await mvcDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}

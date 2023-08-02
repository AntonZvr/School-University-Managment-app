using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> GetStudents(int groupId)
        {
            var students = await _studentService.GetAllStudents(groupId);
            return Json(students);
        }

        public async Task<IActionResult> ChangeStudentName(int studentId, string newFirstName, string newLastName)
        {
            var student = await _studentService.UpdateStudentName(studentId, newFirstName, newLastName);
            return Json(student);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            try
            {
                await _studentService.DeleteStudent(studentId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(int groupId, string studentFirstName, string studentLastName)
        {
            var newStudent = await _studentService.AddStudent(groupId, studentFirstName, studentLastName);
            return Json(newStudent);
        }
    }
}


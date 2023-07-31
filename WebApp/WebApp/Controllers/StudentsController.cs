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
            var students = await _studentService.GetAllStudents();
            return Json(students.Where(s => s.GROUP_ID == groupId).Select(s => new { Id = s.STUDENT_ID, FirstName = s.FIRST_NAME, LastName = s.LAST_NAME }));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStudentName(int studentId, string newFirstName, string newLastName)
        {
            await _studentService.UpdateStudentName(studentId, newFirstName, newLastName);
            var student = await _studentService.GetStudent(studentId);
            return Json(new { Id = student.STUDENT_ID, FirstName = student.FIRST_NAME, LastName = student.LAST_NAME });
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
        public async Task<IActionResult> AddStudent(int groupId, string studentFirstName,
        string studentLastName)
        {
            var newStudent = await _studentService.AddStudent(groupId, studentFirstName, studentLastName);
            return Json(new { Id = newStudent.GROUP_ID, FirstName = newStudent.FIRST_NAME, LastName = newStudent.LAST_NAME });
        }
    }
}


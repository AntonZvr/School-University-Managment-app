using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public HomeController(IGroupService groupService, IStudentService studentService, ICourseService courseService)
        {
            _groupService = groupService;
            _studentService = studentService;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCourses();
            return View(courses);
        }

        public async Task<IActionResult> GetGroups(int courseId)
        {
            var groups = await _groupService.GetAllGroups(courseId);
            return Json(groups.Select(g => new { Id = g.GROUP_ID, Name = g.NAME }));
        }

        public async Task<IActionResult> GetStudents(int groupId)
        {
            var students = await _studentService.GetAllStudents();
            return Json(students.Where(s => s.GROUP_ID == groupId).Select(s => new { Id = s.STUDENT_ID, FirstName = s.FIRST_NAME, LastName = s.LAST_NAME }));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeGroupName(int groupId, string newName)
        {
            await _groupService.UpdateGroupName(groupId, newName);
            var group = await _groupService.GetGroup(groupId);
            return Json(new { Id = group.GROUP_ID, Name = group.NAME });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStudentName(int studentId, string newFirstName, string newLastName)
        {
            await _studentService.UpdateStudentName(studentId, newFirstName, newLastName);
            var student = await _studentService.GetStudent(studentId);
            return Json(new { Id = student.STUDENT_ID, FirstName = student.FIRST_NAME, LastName = student.LAST_NAME });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            try
            {
                await _groupService.DeleteGroup(groupId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }

}

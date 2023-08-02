using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;

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
    }

}

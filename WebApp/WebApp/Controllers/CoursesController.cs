using Microsoft.AspNetCore.Mvc;
using WebApp.Data.ViewModels;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCourses();
            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(string courseName, string description)
        {
            var newCourse = await _courseService.AddCourse(courseName, description);
            return Json(new { Name = newCourse.NAME, Desc = newCourse.DESCRIPTION });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeCourseName(int id, string newName)
        {
            var updatedCourse = await _courseService.ChangeCourseName(id, newName);
            return Json(updatedCourse);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourse(id);
            return Json(new { success = result });
        }
    }
}

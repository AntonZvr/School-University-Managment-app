using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebApp.Data.ViewModels;
using WebApp.Services.Interfaces;

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
            return View(courses.ToList());
        }

        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseService.GetSimpleCourses();
            return Json(courses);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(string courseName, string description)
        {
            var newCourse = await _courseService.AddCourse(courseName, description);
            return Json(newCourse);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourseName(int courseId, string newName)
        {
            var course = await _courseService.UpdateCourseName(courseId, newName);
            return Json(course);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            try
            {
                await _courseService.DeleteCourse(courseId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
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

        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseService.GetAllCourses();
            return Json(courses.Select(c => new { Id = c.COURSE_ID, Name = c.NAME }));
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(string courseName, string description)
        {
            var newCourse = await _courseService.AddCourse(courseName, description);
            return Json(new { Name = newCourse.NAME, Desc = newCourse.DESCRIPTION });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourseName(int courseId, string newName)
        {
            await _courseService.UpdateCourseName(courseId, newName);
            var course = await _courseService.GetCourse(courseId);
            return Json(new { Id = course.COURSE_ID, Name = course.NAME });
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

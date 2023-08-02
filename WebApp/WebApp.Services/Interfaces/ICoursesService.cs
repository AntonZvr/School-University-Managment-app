using WebApp.Data.ViewModels;
using WebApp.Models;

namespace WebApp.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseViewModel>> GetAllCourses();
        Task<object> GetCourse(int courseId);
        Task<IEnumerable<object>> GetSimpleCourses();
        Task DeleteCourse(int courseId);
        Task<object> AddCourse(string courseName, string description);
        Task<object> UpdateCourseName(int courseId, string newName);
    }
}

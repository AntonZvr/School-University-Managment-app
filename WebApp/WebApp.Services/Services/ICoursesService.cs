using WebApp.Data.ViewModels;
using WebApp.Models;

namespace WebApp.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseViewModel>> GetAllCourses();
        Task<CourseViewModel> GetCourse(int courseId);
        Task<bool> DeleteCourse(int id);
        Task<CourseViewModel> AddCourse(string courseName, string description);
        Task UpdateCourseName(int courseId, string newName);
    }
}

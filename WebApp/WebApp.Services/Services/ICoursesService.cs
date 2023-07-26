using WebApp.Data.ViewModels;
using WebApp.Models;

namespace WebApp.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseViewModel>> GetAllCourses();
    }
}

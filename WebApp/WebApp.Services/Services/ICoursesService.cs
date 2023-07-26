using WebApp.Models;

namespace WebApp.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CoursesModel>> GetAllCourses();
    }
}

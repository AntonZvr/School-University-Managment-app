using WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<CoursesModel>> GetAllCourses();
        Task<CoursesModel> GetCourse(int courseId);
        Task<CoursesModel> AddCourse(string courseName, string description);
        Task<CoursesModel> UpdateCourseName(int courseId, string newName);
        Task<bool> HasAssociatedGroups(int courseId);
        Task DeleteCourse(CoursesModel course);
    }

}

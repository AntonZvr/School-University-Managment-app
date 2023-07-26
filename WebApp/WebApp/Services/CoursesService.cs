using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;

        public CourseService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CoursesModel>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }
    }
}

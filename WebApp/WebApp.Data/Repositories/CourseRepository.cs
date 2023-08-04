using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SchoolContext _context;

        public CourseRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CoursesModel>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<CoursesModel> GetCourse(int courseId)
        {
            return await _context.Courses.FindAsync(courseId);
        }

        public async Task<CoursesModel> AddCourse(string courseName, string description)
        {
            var newCourse = new CoursesModel
            {
                DESCRIPTION = description,
                NAME = courseName
            };

            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            return newCourse;
        }


        public async Task<CoursesModel> UpdateCourseName(int courseId, string newName)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course != null)
            {
                course.NAME = newName;
                await _context.SaveChangesAsync();
            }

            return course;
        }

        public async Task<bool> HasAssociatedGroups(int courseId)
        {
            return await _context.Groups.AnyAsync(g => g.COURSE_ID == courseId);
        }

        public async Task DeleteCourse(CoursesModel course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }

}

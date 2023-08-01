using Microsoft.EntityFrameworkCore;
using WebApp.Data.ViewModels;
using WebApp.Models;
using AutoMapper;
using System.Text.RegularExpressions;

namespace WebApp.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public CourseService(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseViewModel>> GetAllCourses()
        {
            var courses = await _context.Courses.ToListAsync();
            var courseViewModels = _mapper.Map<List<CourseViewModel>>(courses);

            return courseViewModels;
        }

        public async Task<CourseViewModel> GetCourse(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return null;
            }

            var courseViewModel = _mapper.Map<CourseViewModel>(course);

            return courseViewModel;
        }

        public async Task<CourseViewModel> AddCourse(string courseName, string description)
        {           
            var newCourse = new CoursesModel
            {
                DESCRIPTION = description,
                NAME = courseName
            };

            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            var courseViewModel = _mapper.Map<CourseViewModel>(newCourse);

            return courseViewModel;
        }

        public async Task<bool> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task UpdateCourseName(int courseId, string newName)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course != null)
            {
                course.NAME = newName;
                await _context.SaveChangesAsync();
            }
        }
    }
}

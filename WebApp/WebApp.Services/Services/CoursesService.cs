using Microsoft.EntityFrameworkCore;
using WebApp.Data.ViewModels;
using WebApp.Models;
using AutoMapper;
using System.Text.RegularExpressions;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public CourseService(IRepository<CoursesModel> courseRepository, IMapper mapper)
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

        public async Task<IEnumerable<object>> GetSimpleCourses()
        {
            var courses = await _context.Courses.ToListAsync();
            var courseViewModels = _mapper.Map<List<CourseViewModel>>(courses);

            var result = courseViewModels.Select(c => new { Id = c.COURSE_ID, Name = c.NAME });

            return result;
        }

        public async Task<object> GetCourse(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return null;
            }

            var courseViewModel = _mapper.Map<CourseViewModel>(course);

            return new { Id = courseViewModel.COURSE_ID, Name = courseViewModel.NAME };
        }

        public async Task<object> AddCourse(string courseName, string description)
        {
            var newCourse = new CoursesModel
            {
                DESCRIPTION = description,
                NAME = courseName
            };

            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            var courseViewModel = _mapper.Map<CourseViewModel>(newCourse);

            return new { Name = courseViewModel.NAME, Desc = courseViewModel.DESCRIPTION };
        }

        public async Task<object> UpdateCourseName(int courseId, string newName)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course != null)
            {
                course.NAME = newName;
                await _context.SaveChangesAsync();
            }

            var courseViewModel = _mapper.Map<CourseViewModel>(course);

            return new { Id = courseViewModel.COURSE_ID, Name = courseViewModel.NAME };
        }

        public async Task DeleteCourse(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course != null)
            {
                var groups = await _context.Groups.Where(g => g.COURSE_ID == courseId).ToListAsync();
                if (groups.Count == 0)
                {
                    _context.Courses.Remove(course);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException($"Course {courseId} includes groups and can't be deleted.");
                }
            }
        }
    }
}

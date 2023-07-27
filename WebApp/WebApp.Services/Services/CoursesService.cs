using Microsoft.EntityFrameworkCore;
using WebApp.Data.ViewModels;
using WebApp.Models;
using AutoMapper;

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
    }
}

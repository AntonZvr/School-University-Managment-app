using WebApp.Data.ViewModels;
using WebApp.Models;
using AutoMapper;
using System.Text.RegularExpressions;
using WebApp.Services.Interfaces;
using WebApp.Repositories;

namespace WebApp.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseViewModel>> GetAllCourses()
        {
            var courses = await _courseRepository.GetAllCourses();
            var courseViewModels = _mapper.Map<List<CourseViewModel>>(courses);
            return courseViewModels;
        }

        public async Task<object> GetCourse(int courseId)
        {
            var course = await _courseRepository.GetCourse(courseId);

            if (course == null)
            {
                return null;
            }

            var courseViewModel = _mapper.Map<CourseViewModel>(course);

            return new { Id = courseViewModel.COURSE_ID, Name = courseViewModel.NAME };
        }

        public async Task<IEnumerable<object>> GetSimpleCourses()
        {
            var courses = await _courseRepository.GetAllCourses();
            var courseViewModels = _mapper.Map<List<CourseViewModel>>(courses);

            var result = courseViewModels.Select(c => new { Id = c.COURSE_ID, Name = c.NAME });

            return result;
        }

        public async Task<object> AddCourse(string courseName, string description)
        {
            var addedCourse = await _courseRepository.AddCourse(courseName, description);
            var courseViewModel = _mapper.Map<CourseViewModel>(addedCourse);

            return new { Name = courseViewModel.NAME, Desc = courseViewModel.DESCRIPTION };
        }

        public async Task<object> UpdateCourseName(int courseId, string newName)
        {
            var updatedCourse = await _courseRepository.UpdateCourseName(courseId, newName);

            if (updatedCourse == null)
            {
                return null;
            }

            var courseViewModel = _mapper.Map<CourseViewModel>(updatedCourse);

            return new { Id = courseViewModel.COURSE_ID, Name = courseViewModel.NAME };
        }

        public async Task DeleteCourse(int courseId)
        {
            var course = await _courseRepository.GetCourse(courseId);

            if (course != null)
            {
                var hasAssociatedGroups = await _courseRepository.HasAssociatedGroups(courseId);

                if (!hasAssociatedGroups)
                {
                    await _courseRepository.DeleteCourse(course);
                }
                else
                {
                    throw new ArgumentException($"Course {courseId} includes groups and can't be deleted.");
                }
            }
        }
    }

}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApp.Data.ViewModels;
using WebApp.Models;
using AutoMapper;
using WebApp.Services.Interfaces;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Tests
{
    [TestClass]
    public class CourseServiceTests
    {
        private Mock<ICourseRepository> _mockCourseRepository;
        private CourseService _courseService;
        private Mock<IMapper> _mapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();

            // Initialize AutoMapper if needed
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoursesModel, CourseViewModel>();
                // Add more mappings if needed
            });
            var mapper = mapperConfig.CreateMapper();

            _courseService = new CourseService(_mockCourseRepository.Object, mapper);
            
            _mapper = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task GetAllCourses_ShouldReturnAllCourses()
        {
            // Arrange
            var expectedCourses = new List<CoursesModel>
            {
                new CoursesModel { COURSE_ID = 1, NAME = "Course 1", DESCRIPTION = "Description 1" },
                new CoursesModel { COURSE_ID = 2, NAME = "Course 2", DESCRIPTION = "Description 2" }
            };

            _mockCourseRepository.Setup(r => r.GetAllCourses()).ReturnsAsync(expectedCourses);

            // Act
            var result = await _courseService.GetAllCourses();

            // Assert
            Assert.AreEqual(expectedCourses.Count, result.Count());
            // Add more assertions if needed
        }

        [TestMethod]
        public async Task GetCourse_WithValidCourseId_ShouldReturnCourseViewModel()
        {
            // Arrange
            int courseId = 1;
            var expectedCourse = new CoursesModel { COURSE_ID = courseId, NAME = "Course 1", DESCRIPTION = "Description 1" };

            _mockCourseRepository.Setup(r => r.GetCourse(courseId)).ReturnsAsync(expectedCourse);

            // Act
            var result = await _courseService.GetCourse(courseId);

            // Assert
            Assert.IsNotNull(result);
            // Add more assertions if needed
        }

        [TestMethod]
        public async Task GetCourse_WithInvalidCourseId_ShouldReturnNull()
        {
            // Arrange
            int courseId = 1;
            CoursesModel expectedCourse = null;

            _mockCourseRepository.Setup(r => r.GetCourse(courseId)).ReturnsAsync(expectedCourse);

            // Act
            var result = await _courseService.GetCourse(courseId);

            // Assert
            Assert.IsNull(result);         
        }

        [TestMethod]
        public async Task AddCourse_ShouldReturnAddedCourseViewModel()
        {
            // Arrange
            string courseName = "New Course";
            string description = "Course Description";
            var addedCourseModel = new CoursesModel
            {
                COURSE_ID = 1,
                NAME = courseName,
                DESCRIPTION = description
            };

            var addedCourses = new List<CoursesModel>();
            _mockCourseRepository.Setup(r => r.AddCourse(courseName, description))
                .Callback<string, string>((name, desc) =>
                {
                    addedCourses.Add(new CoursesModel
                    {
                        COURSE_ID = addedCourseModel.COURSE_ID,
                        NAME = addedCourseModel.NAME,
                        DESCRIPTION = addedCourseModel.DESCRIPTION
                    });
                })
                .ReturnsAsync(addedCourseModel);

            // Act
            var result = await _courseService.AddCourse(courseName, description);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(addedCourses.Any(c => c.NAME == courseName && c.DESCRIPTION == description));          
        }

        [TestMethod]
        public async Task UpdateCourseName_ShouldReturnUpdatedCourse()
        {
            // Arrange
            var courseId = 1;
            var newName = "New Course Name";
            var course = new CoursesModel { COURSE_ID = courseId, NAME = "Old Course Name" };
            var updatedCourse = new CoursesModel { COURSE_ID = courseId, NAME = newName };
            var courseViewModel = new CourseViewModel { COURSE_ID = courseId, NAME = newName };

            _mockCourseRepository.Setup(x => x.UpdateCourseName(courseId, newName)).ReturnsAsync(updatedCourse);
            _mapper.Setup(x => x.Map<CourseViewModel>(updatedCourse)).Returns(courseViewModel);

            // Act
            var result = await _courseService.UpdateCourseName(courseId, newName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(courseId, result.Id);
            Assert.AreEqual(newName, result.Name);
        }

        [TestMethod]
        public async Task DeleteCourse_ShouldDeleteCourse_WhenNoAssociatedGroups()
        {
            // Arrange
            var courseId = 1;
            var course = new CoursesModel { COURSE_ID = courseId, NAME = "Course Name" };

            _mockCourseRepository.Setup(x => x.GetCourse(courseId)).ReturnsAsync(course);
            _mockCourseRepository.Setup(x => x.HasAssociatedGroups(courseId)).ReturnsAsync(false);

            // Act
            await _courseService.DeleteCourse(courseId);

            // Assert
            _mockCourseRepository.Verify(x => x.DeleteCourse(course), Times.Once);
        }

        [TestMethod]
        public async Task DeleteCourse_ShouldThrowException_WhenHasAssociatedGroups()
        {
            // Arrange
            var courseId = 1;
            var course = new CoursesModel { COURSE_ID = courseId, NAME = "Course Name" };

            _mockCourseRepository.Setup(x => x.GetCourse(courseId)).ReturnsAsync(course);
            _mockCourseRepository.Setup(x => x.HasAssociatedGroups(courseId)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _courseService.DeleteCourse(courseId));
        }

    }
}

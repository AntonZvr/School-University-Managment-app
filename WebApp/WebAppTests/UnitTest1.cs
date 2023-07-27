using WebApp.Controllers;
using WebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApp.Data.ViewModels;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace WebAppTests
{
    [TestClass]
    public class StudentServiceTests
    {
        private SchoolContext _context;
        private IMapper _mapper;
        private StudentService _studentService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase(databaseName: "StudentTestDb")
                .Options;

            _context = new SchoolContext(options);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _studentService = new StudentService(_context, _mapper);

            // Add test student data
            _context.Students.Add(new StudentsModel { STUDENT_ID = 1, FIRST_NAME = "John", LAST_NAME = "Doe" });
            _context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAllStudents_ReturnsAllStudents()
        {
            var students = await _studentService.GetAllStudents();
            Assert.AreEqual(1, students.Count());
        }

        [TestMethod]
        public async Task GetStudent_ReturnsStudent()
        {
            var student = await _studentService.GetStudent(1);
            Assert.AreEqual("John", student.FIRST_NAME);
        }

        [TestMethod]
        public async Task UpdateStudentName_ReturnsUpdatedStudent()
        {
            await _studentService.UpdateStudentName(1, "Jane", "Doe");
            var student = await _studentService.GetStudent(1);
            Assert.AreEqual("Jane", student.FIRST_NAME);
        }
    }

    [TestClass]
    public class CourseServiceTests
    {
        private SchoolContext _context;
        private IMapper _mapper;
        private CourseService _courseService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase(databaseName: "CourseTestDb")
                .Options;

            _context = new SchoolContext(options);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _courseService = new CourseService(_context, _mapper);

            // Add test course data
            _context.Courses.Add(new CoursesModel { COURSE_ID = 1, NAME = "Math", DESCRIPTION = "this is a course description" });
            _context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAllCourses_ReturnsAllCourses()
        {
            var courses = await _courseService.GetAllCourses();
            Assert.AreEqual(1, courses.Count());
        }
    }

    [TestClass]
    public class GroupServiceTests
    {
        private SchoolContext _context;
        private IMapper _mapper;
        private GroupService _groupService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase(databaseName: "GroupTestDb")
                .Options;

            _context = new SchoolContext(options);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _groupService = new GroupService(_context, _mapper);

            // Add test group data (including a student and course)
            _context.Groups.Add(new GroupsModel { GROUP_ID = 1, NAME = "GroupA", COURSE_ID = 1 });
            _context.Students.Add(new StudentsModel { STUDENT_ID = 1, FIRST_NAME = "John", LAST_NAME = "Doe", GROUP_ID = 2 });
            _context.Courses.Add(new CoursesModel { COURSE_ID = 1, NAME = "CourseA", DESCRIPTION = "this is a course description" });
            _context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAllGroups_ReturnsAllGroups()
        {
            var groups = await _groupService.GetAllGroups(1);
            Assert.AreEqual(1, groups.Count());
        }

        [TestMethod]
        public async Task GetGroup_ReturnsGroup()
        {
            var group = await _groupService.GetGroup(1);
            Assert.AreEqual("GroupA", group.NAME);
        }

        [TestMethod]
        public async Task UpdateGroupName_ReturnsUpdatedGroup()
        {
            await _groupService.UpdateGroupName(1, "GroupB");
            var group = await _groupService.GetGroup(1);
            Assert.AreEqual("GroupB", group.NAME);
        }

        [TestMethod]
        public async Task DeleteGroup_NoStudents_ReturnsDeletedGroup()
        {
            await _groupService.DeleteGroup(1);
            var group = await _groupService.GetGroup(1);
            Assert.IsNull(group);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task DeleteGroup_WithStudents_ThrowsException()
        {
            _context.Students.Add(new StudentsModel { STUDENT_ID = 2, FIRST_NAME = "Jane", LAST_NAME = "Doe", GROUP_ID = 1 });
            _context.SaveChanges();
            await _groupService.DeleteGroup(1);
        }
    }

}
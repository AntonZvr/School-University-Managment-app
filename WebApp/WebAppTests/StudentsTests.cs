using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data.ViewModels;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services.Interfaces;

namespace WebApp.Tests
{
    [TestClass]
    public class StudentServiceTests
    {
        private Mock<IStudentRepository> _studentRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IStudentService _studentService;

        [TestInitialize]
        public void Initialize()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _mapperMock = new Mock<IMapper>();
            _studentService = new StudentService(_studentRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task GetAllStudents_ReturnsStudentViewModels()
        {
            // Arrange
            int groupId = 1;
            var students = new List<StudentsModel>
            {
                new StudentsModel { STUDENT_ID = 1, FIRST_NAME = "John", LAST_NAME = "Doe" },
                new StudentsModel { STUDENT_ID = 2, FIRST_NAME = "Jane", LAST_NAME = "Smith" }
            };
            var studentViewModels = new List<StudentsViewModel>
            {
                new StudentsViewModel { STUDENT_ID = 1, FIRST_NAME = "John", LAST_NAME = "Doe" },
                new StudentsViewModel { STUDENT_ID = 2, FIRST_NAME = "Jane", LAST_NAME = "Smith" }
            };

            _studentRepositoryMock.Setup(repo => repo.GetAllStudents(groupId)).ReturnsAsync(students);
            _mapperMock.Setup(mapper => mapper.Map<List<StudentsViewModel>>(students)).Returns(studentViewModels);

            // Act
            var result = await _studentService.GetAllStudents(groupId);

            // Assert
            Assert.AreEqual(studentViewModels.Count, result.Count());
            foreach (var expectedStudent in studentViewModels)
            {
                var actualStudent = result.FirstOrDefault(s => s.GetType().GetProperty("Id").GetValue(s).Equals(expectedStudent.STUDENT_ID) && s.GetType().GetProperty("FirstName").GetValue(s).Equals(expectedStudent.FIRST_NAME) && s.GetType().GetProperty("LastName").GetValue(s).Equals(expectedStudent.LAST_NAME));
                Assert.IsNotNull(actualStudent);
            }
        }

        [TestMethod]
        public async Task GetStudent_ExistingStudentId_ReturnsStudentViewModel()
        {
            // Arrange
            int studentId = 1;
            var student = new StudentsModel { STUDENT_ID = studentId, FIRST_NAME = "John", LAST_NAME = "Doe" };
            var studentViewModel = new StudentsViewModel { STUDENT_ID = studentId, FIRST_NAME = "John", LAST_NAME = "Doe" };

            _studentRepositoryMock.Setup(repo => repo.GetStudent(studentId)).ReturnsAsync(student);
            _mapperMock.Setup(mapper => mapper.Map<StudentsViewModel>(student)).Returns(studentViewModel);

            // Act
            var result = await _studentService.GetStudent(studentId);

            // Assert
            Assert.AreEqual(studentViewModel.STUDENT_ID, result.GetType().GetProperty("Id").GetValue(result));
            Assert.AreEqual(studentViewModel.FIRST_NAME, result.GetType().GetProperty("FirstName").GetValue(result));
            Assert.AreEqual(studentViewModel.LAST_NAME, result.GetType().GetProperty("LastName").GetValue(result));
        }

        [TestMethod]
        public async Task GetStudent_NonExistingStudentId_ReturnsNull()
        {
            // Arrange
            int studentId = 1;

            _studentRepositoryMock.Setup(repo => repo.GetStudent(studentId)).ReturnsAsync((StudentsModel)null);

            // Act
            var result = await _studentService.GetStudent(studentId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateStudentName_ExistingStudentId_ReturnsUpdatedStudentViewModel()
        {
            // Arrange
            int studentId = 1;
            string newFirstName = "Updated First Name";
            string newLastName = "Updated Last Name";
            var student = new StudentsModel { STUDENT_ID = studentId, FIRST_NAME = "John", LAST_NAME = "Doe" };
            var updatedStudent = new StudentsModel { STUDENT_ID = studentId, FIRST_NAME = newFirstName, LAST_NAME = newLastName };
            var updatedStudentViewModel = new StudentsViewModel { STUDENT_ID = studentId, FIRST_NAME = newFirstName, LAST_NAME = newLastName };

            _studentRepositoryMock.Setup(repo => repo.UpdateStudentName(studentId, newFirstName, newLastName)).ReturnsAsync(updatedStudent);
            _mapperMock.Setup(mapper => mapper.Map<StudentsViewModel>(updatedStudent)).Returns(updatedStudentViewModel);

            // Act
            var result = await _studentService.UpdateStudentName(studentId, newFirstName, newLastName);

            // Assert
            Assert.AreEqual(updatedStudentViewModel.STUDENT_ID, result.GetType().GetProperty("Id").GetValue(result));
            Assert.AreEqual(updatedStudentViewModel.FIRST_NAME, result.GetType().GetProperty("FirstName").GetValue(result));
            Assert.AreEqual(updatedStudentViewModel.LAST_NAME, result.GetType().GetProperty("LastName").GetValue(result));
        }

        [TestMethod]
        public async Task AddStudent_ReturnsNewStudentViewModel()
        {
            // Arrange
            int groupId = 1;
            string studentFirstName = "John";
            string studentLastName = "Doe";
            var newStudent = new StudentsModel { STUDENT_ID = 3, FIRST_NAME = studentFirstName, LAST_NAME = studentLastName };
            var newStudentViewModel = new StudentsViewModel { STUDENT_ID = 3, FIRST_NAME = studentFirstName, LAST_NAME = studentLastName };

            _studentRepositoryMock.Setup(repo => repo.AddStudent(groupId, studentFirstName, studentLastName)).ReturnsAsync(newStudent);
            _mapperMock.Setup(mapper => mapper.Map<StudentsViewModel>(newStudent)).Returns(newStudentViewModel);

            // Act
            var result = await _studentService.AddStudent(groupId, studentFirstName, studentLastName);

            // Assert
            Assert.AreEqual(newStudentViewModel.STUDENT_ID, result.GetType().GetProperty("Id").GetValue(result));
            Assert.AreEqual(newStudentViewModel.FIRST_NAME, result.GetType().GetProperty("FirstName").GetValue(result));
            Assert.AreEqual(newStudentViewModel.LAST_NAME, result.GetType().GetProperty("LastName").GetValue(result));
        }

        [TestMethod]
        public async Task DeleteStudent_DeletesStudent()
        {
            // Arrange
            int studentId = 1;

            _studentRepositoryMock.Setup(repo => repo.GetStudent(studentId)).ReturnsAsync(new StudentsModel { STUDENT_ID = studentId });

            // Act
            await _studentService.DeleteStudent(studentId);

            // Assert
            _studentRepositoryMock.Verify(repo => repo.DeleteStudent(It.IsAny<StudentsModel>()), Times.Once);
        }

    }
}

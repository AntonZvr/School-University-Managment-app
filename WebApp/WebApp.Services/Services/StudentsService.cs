using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Data.ViewModels;
using AutoMapper;

namespace WebApp.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public StudentService(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<object>> GetAllStudents(int groupId)
        {
            var students = await _context.Students.Where(s => s.GROUP_ID == groupId).ToListAsync();
            var studentViewModels = _mapper.Map<List<StudentsViewModel>>(students);

            return studentViewModels.Select(s => new { Id = s.STUDENT_ID, FirstName = s.FIRST_NAME, LastName = s.LAST_NAME });
        }

        public async Task<object> GetStudent(int studentId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.STUDENT_ID == studentId);

            if (student == null)
            {
                return null;
            }
            var studentViewModel = _mapper.Map<StudentsViewModel>(student);

            return new { Id = studentViewModel.STUDENT_ID, FirstName = studentViewModel.FIRST_NAME, LastName = studentViewModel.LAST_NAME };
        }

        public async Task<object> UpdateStudentName(int studentId, string newFirstName, string newLastName)
        {
            var student = await _context.Students.FindAsync(studentId);

            if (student != null)
            {
                if (!string.IsNullOrEmpty(newFirstName))
                {
                    student.FIRST_NAME = newFirstName;
                }
                if (!string.IsNullOrEmpty(newLastName))
                {
                    student.LAST_NAME = newLastName;
                }

                await _context.SaveChangesAsync();
            }

            var studentViewModel = _mapper.Map<StudentsViewModel>(student);

            return new { Id = studentViewModel.STUDENT_ID, FirstName = studentViewModel.FIRST_NAME, LastName = studentViewModel.LAST_NAME };
        }

        public async Task DeleteStudent(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);

            if (student != null)
            {
                var students = await _context.Students.Where(s => s.STUDENT_ID == studentId).ToListAsync();

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Argument error");
            }
        }

        public async Task<object> AddStudent(int groupId, string studentFirstName, string studentLastName)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                throw new ArgumentException("Group not found.");
            }

            var newStudent = new StudentsModel
            {
                GROUP_ID = groupId,
                FIRST_NAME = studentFirstName,
                LAST_NAME = studentLastName
            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            var studentViewModel = _mapper.Map<StudentsViewModel>(newStudent);

            return new { Id = studentViewModel.STUDENT_ID, FirstName = studentViewModel.FIRST_NAME, LastName = studentViewModel.LAST_NAME };
        }
    }


}

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

        public async Task<IEnumerable<StudentsViewModel>> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            var studentViewModels = _mapper.Map<List<StudentsViewModel>>(students);

            return studentViewModels;
        }

        public async Task<StudentsViewModel> GetStudent(int studentId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.STUDENT_ID == studentId);

            if (student == null)
            {
                return null;
            }
            var studentViewModel = _mapper.Map<StudentsViewModel>(student);

            return studentViewModel;
        }

        public async Task UpdateStudentName(int studentId, string newFirstName, string newLastName)
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

        public async Task<StudentsViewModel> AddStudent(int groupId, string studentFirstName, string studentLastName)
        {
            var student = await _context.Students.FindAsync(groupId);
            if (student == null)
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

            return studentViewModel;
        }
    }

}

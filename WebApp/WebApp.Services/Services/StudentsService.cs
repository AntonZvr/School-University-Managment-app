using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApp.Models;
using WebApp.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
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
    }

}

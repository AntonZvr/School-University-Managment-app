using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;

        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentsModel>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<StudentsModel> GetStudent(int studentId)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.STUDENT_ID == studentId);
        }

        public async Task UpdateStudentName(int studentId, string newFirstName, string newLastName)
        {
            var student = await GetStudent(studentId);

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

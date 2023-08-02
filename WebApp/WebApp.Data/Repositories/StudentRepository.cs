using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentsModel>> GetAllStudents(int groupId)
        {
            return await _context.Students.Where(s => s.GROUP_ID == groupId).ToListAsync();
        }

        public async Task<StudentsModel> GetStudent(int studentId)
        {
            return await _context.Students.FindAsync(studentId);
        }

        public async Task<StudentsModel> AddStudent(int groupId, string studentFirstName, string studentLastName)
        {
            var newStudent = new StudentsModel
            {
                GROUP_ID = groupId,
                FIRST_NAME = studentFirstName,
                LAST_NAME = studentLastName
            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            return newStudent;
        }

        public async Task<StudentsModel> UpdateStudentName(int studentId, string newFirstName, string newLastName)
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

            return student;
        }

        public async Task DeleteStudent(StudentsModel student)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}

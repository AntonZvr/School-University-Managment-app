using WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentsModel>> GetAllStudents(int groupId);
        Task<StudentsModel> GetStudent(int studentId);
        Task<StudentsModel> AddStudent(int groupId, string studentFirstName, string studentLastName);
        Task<StudentsModel> UpdateStudentName(int studentId, string newFirstName, string newLastName);
        Task DeleteStudent(StudentsModel student);
    }
}

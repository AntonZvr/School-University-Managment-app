using WebApp.Models;

namespace WebApp.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentsModel>> GetAllStudents();
        Task<StudentsModel> GetStudent(int studentId);
        Task UpdateStudentName(int studentId, string newFirstName, string newLastName);
    }
}

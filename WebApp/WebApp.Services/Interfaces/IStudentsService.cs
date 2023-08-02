using WebApp.Data.ViewModels;

public interface IStudentService
{
    Task<IEnumerable<object>> GetAllStudents(int groupId);
    Task<object> GetStudent(int studentId);
    Task<object> UpdateStudentName(int studentId, string newFirstName, string newLastName);
    Task DeleteStudent(int studentId);
    Task<object> AddStudent(int groupId, string studentFirstName, string studentLastName);
}

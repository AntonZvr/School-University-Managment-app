using WebApp.Data.ViewModels;

public interface IStudentService
{
    Task<IEnumerable<StudentsViewModel>> GetAllStudents();
    Task<StudentsViewModel> GetStudent(int studentId);
    Task UpdateStudentName(int studentId, string newFirstName, string newLastName);
    Task DeleteStudent(int studentId);
    Task<StudentsViewModel> AddStudent(int groupId, string studentFirstName, 
        string studentLastName);
}

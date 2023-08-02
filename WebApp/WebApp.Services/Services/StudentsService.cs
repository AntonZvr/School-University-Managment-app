using AutoMapper;
using WebApp.Data.ViewModels;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentService(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<object>> GetAllStudents(int groupId)
    {
        var students = await _studentRepository.GetAllStudents(groupId);
        var studentViewModels = _mapper.Map<List<StudentsViewModel>>(students);

        return studentViewModels.Select(s => new { Id = s.STUDENT_ID, FirstName = s.FIRST_NAME, LastName = s.LAST_NAME });
    }

    public async Task<object> GetStudent(int studentId)
    {
        var student = await _studentRepository.GetStudent(studentId);

        if (student == null)
        {
            return null;
        }

        var studentViewModel = _mapper.Map<StudentsViewModel>(student);

        return new { Id = studentViewModel.STUDENT_ID, FirstName = studentViewModel.FIRST_NAME, LastName = studentViewModel.LAST_NAME };
    }

    public async Task<object> UpdateStudentName(int studentId, string newFirstName, string newLastName)
    {
        var student = await _studentRepository.UpdateStudentName(studentId, newFirstName, newLastName);

        var studentViewModel = _mapper.Map<StudentsViewModel>(student);

        return new { Id = studentViewModel.STUDENT_ID, FirstName = studentViewModel.FIRST_NAME, LastName = studentViewModel.LAST_NAME };
    }

    public async Task DeleteStudent(int studentId)
    {
        var student = await _studentRepository.GetStudent(studentId);

        if (student != null)
        {
            await _studentRepository.DeleteStudent(student);
        }
        else
        {
            throw new ArgumentException("Student not found.");
        }
    }

    public async Task<object> AddStudent(int groupId, string studentFirstName, string studentLastName)
    {
        var newStudent = await _studentRepository.AddStudent(groupId, studentFirstName, studentLastName);

        var studentViewModel = _mapper.Map<StudentsViewModel>(newStudent);

        return new { Id = studentViewModel.STUDENT_ID, FirstName = studentViewModel.FIRST_NAME, LastName = studentViewModel.LAST_NAME };
    }
}

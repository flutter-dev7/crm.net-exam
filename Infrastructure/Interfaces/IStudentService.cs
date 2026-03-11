using System;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IStudentService
{
    Task<List<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task<string> AddStudentAsync(Student student);
    Task<string> UpdateStudentAsync(int id, Student student);
    Task<string> DeleteStudentAsync(int id);
}

using System;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IStudentGroupService
{
    Task<List<Student>> GetStudentsByGroupIdAsync(int groupId);
    Task<string> AddStudentGroupAsync(int groupId, int studentId);
    Task<string> DeleteStudentGroupAsync(int groupId, int studentId);
}

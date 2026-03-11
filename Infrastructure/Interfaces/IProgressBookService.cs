using System;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IProgressBookService
{
    Task<List<ProgressBook>> GetStudentProgressBook(int studentId);
    Task<List<ProgressBook>> GetGroupProgressBook(int groupId);
    Task<string> AddProgressBookAsync(ProgressBook progressBook);
    Task<string> UpdateProgressBookAsync(int id, ProgressBook progressBook);
}

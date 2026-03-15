using System;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController(StudentService service)
{
    [HttpGet]
    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await service.GetAllStudentsAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        return await service.GetStudentByIdAsync(id);
    }

    [HttpGet("{id:int}/average-grade")]
    public async Task<int> GetAverageGradeAsync(int id)
    {
        return await service.GetAverageGradeAsync(id);
    }

    [HttpGet("{studentId:int}/attendance")]
    public async Task<double> GetAttendancePercentageAsync(int studentId)
    {
        return await service.GetAttendancePercentageAsync(studentId);
    }

    [HttpPost]
    public async Task<string> AddStudentAsync(Student student)
    {
        return await service.AddStudentAsync(student);
    }

    [HttpPut("{id:int}")]
    public async Task<string> UpdateStudentAsync(int id, Student student)
    {
        return await service.UpdateStudentAsync(id, student);
    }

    [HttpDelete("{id:int}")]
    public async Task<string> DeleteStudentAsync(int id)
    {
        return await service.DeleteStudentAsync(id);
    }
}

using System;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/groups")]
public class StudentGroupController(IStudentGroupService service)
{
    [HttpGet("{groupId:int}/students")]
    public async Task<List<Student>> GetStudentsByGroupIdAsync(int groupId)
    {
        return await service.GetStudentsByGroupIdAsync(groupId);
    }

    [HttpPost("{groupId:int}/students/{studentId:int}")]
    public async Task<string> AddStudentGroupAsync(int groupId, int studentId)
    {
        return await service.AddStudentGroupAsync(groupId, studentId);
    }

    [HttpDelete("{groupId:int}/students/{studentId:int}")]
    public async Task<string> DeleteStudentGroupAsync(int groupId, int studentId)
    {
        return await service.DeleteStudentGroupAsync(groupId, studentId);
    }
}

using System;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/")]
public class ProgressBookController
{
    private readonly IProgressBookService service = new ProgressBookService();

    [HttpGet("students/{studentId:int}/progress")]
    public async Task<List<ProgressBook>> GetStudentProgressBook(int studentId)
    {
        return await service.GetStudentProgressBook(studentId);
    }

    [HttpGet("groups/{groupId:int}/progress")]
    public async Task<List<ProgressBook>> GetGroupProgressBook(int groupId)
    {
        return await service.GetGroupProgressBook(groupId);
    }

    [HttpPost("progress")]
    public async Task<string> AddProgressBookAsync(ProgressBook progressBook)
    {
        return await service.AddProgressBookAsync(progressBook);
    }

    [HttpPut("progress/{id:int}")]
    public async Task<string> UpdateProgressBookAsync(int id, ProgressBook progressBook)
    {
        return await service.UpdateProgressBookAsync(id, progressBook);
    }
}

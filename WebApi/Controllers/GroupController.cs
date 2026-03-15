using System;
using Domain.DTO;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupController(GroupService service)
{
    [HttpGet]
    public async Task<List<Group>> GetAllGroupsAsync()
    {
        return await service.GetAllGroupsAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<Group?> GetGroupByIdAsync(int id)
    {
        return await service.GetGroupByIdAsync(id);
    }

    [HttpGet("{groupId:int}/top-students")]
    public async Task<List<BestStudentInGroupDTOs>> GetTopStudentsAsync(int groupId)
    {
        return await service.GetTopStudentsAsync(groupId);
    }

    [HttpPost]
    public async Task<string> AddGroupAsync(Group group)
    {
        return await service.AddGroupAsync(group);
    }

    [HttpPut("{id:int}")]
    public async Task<string> UpdateGroupAsync(int id, Group group)
    {
        return await service.UpdateGroupAsync(id, group);
    }

    [HttpDelete("{id:int}")]
    public async Task<string> DeleteGroupAsync(int id)
    {
        return await service.DeleteGroupAsync(id);
    }
}

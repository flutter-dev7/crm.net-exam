using System;
using Domain.Entities;
namespace Infrastructure.Interfaces;

public interface IGroupService
{
    Task<List<Group>> GetAllGroupsAsync();
    Task<Group?> GetGroupByIdAsync(int id);
    Task<string> AddGroupAsync(Group group);
    Task<string> UpdateGroupAsync(int id, Group group);
    Task<string> DeleteGroupAsync(int id);
}

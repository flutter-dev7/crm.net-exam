using System;
using Dapper;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Npgsql;
using Domain.Entities;
using Domain.DTO;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class GroupService : IGroupService
{
    private readonly DataContext _context;
    private readonly ILogger<GroupService> _logger;

    public GroupService(DataContext context, ILogger<GroupService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Group>> GetAllGroupsAsync()
    {
        try
        {
            using (NpgsqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                string sql = "SELECT * FROM Groups";

                var groups = await connection.QueryAsync<Group>(sql);

                _logger.LogInformation($"Получены все группы, count = {groups.Count()}");

                return groups.ToList();
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении всех групп");
            throw;
        }
    }

    public async Task<Group?> GetGroupByIdAsync(int id)
    {

        try
        {
            _logger.LogTrace("Метод GetGroupByIdAsync начал выполнение");

            _logger.LogDebug("Получен параметр id = {Id}", id);

            using (NpgsqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT * FROM Groups
                WHERE Id = @id";

                var group = await connection.QuerySingleOrDefaultAsync<Group>(sql, new { id });

                if (group == null)
                {
                    _logger.LogWarning("Группа с Id = {Id} не найдена", id);
                }
                else
                {
                    _logger.LogInformation("Группа с Id = {Id} успешно получена", id);
                }

                return group;
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении группы с Id = {Id}", id);
            throw;
        }
    }

    public async Task<List<BestStudentInGroupDTOs>> GetTopStudentsAsync(int groupId)
    {
        try
        {
            using (NpgsqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT StudentId, AVG(Grade) AS Grade
                FROM ProgressBook
                WHERE GroupId = @groupId
                GROUP BY StudentId
                HAVING AVG(Grade) > 80
                ORDER BY Grade DESC";

                var res = await connection.QueryAsync<BestStudentInGroupDTOs>(sql, new { groupId });

                return res.ToList();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> AddGroupAsync(Group group)
    {
        try
        {
            _logger.LogDebug("Начало добавления группы: {GroupName}", group.Name);
            using (NpgsqlConnection connection = _context.GetConnection())
            {
                connection.Open();
                _logger.LogInformation("Соединение с базой открыто");


                if (group.StartDate >= group.EndDate)
                {
                    _logger.LogWarning("StartDate {StartDate} >= EndDate {EndDate} для группы {GroupName}",
                                  group.StartDate, group.EndDate, group.Name);
                    return "StartDate must be less than EndDate";
                }


                string sql = @"
                INSERT INTO Groups (Name, StartDate, EndDate)
                VALUES (@Name, @StartDate, @EndDate)";

                var res = await connection.ExecuteAsync(sql, group);

                if (res == 0)
                {
                    _logger.LogWarning("Группа не создана: {GroupName}", group.Name);
                    return "Group Not Found";
                }


                _logger.LogInformation("Группа успешно создана: {GroupName}", group.Name);
                return "Group Created Successfully";
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении группы: {GroupName}", group.Name);
            throw;
        }
    }

    public async Task<string> UpdateGroupAsync(int id, Group group)
    {
        try
        {
            using (NpgsqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                string sql = @"
                UPDATE Groups SET
                Name = @Name,
                StartDate = @StartDate,
                EndDate = @EndDate
                WHERE Id = @id";

                var res = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    Name = group.Name,
                    StartDate = group.StartDate,
                    EndDate = group.EndDate
                });

                return res == 0 ? "Group Not Found" : "Group Updated Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> DeleteGroupAsync(int id)
    {
        try
        {
            using (NpgsqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                string sql = @"
                DELETE FROM Groups
                WHERE Id = @id";

                var res = await connection.ExecuteAsync(sql, new { id });

                return res == 0 ? "Group Not Found" : "Group Deleted Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}

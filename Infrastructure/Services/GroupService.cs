using System;
using Dapper;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Npgsql;
using Domain.Entities;
using Domain.DTO;

namespace Infrastructure.Services;

public class GroupService : IGroupService
{
    private readonly DataContext context = new();

    public async Task<List<Group>> GetAllGroupsAsync()
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = "SELECT * FROM Groups";

                var groups = await connection.QueryAsync<Group>(sql);

                return groups.ToList();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<Group?> GetGroupByIdAsync(int id)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT * FROM Groups
                WHERE Id = @id";

                var group = await connection.QuerySingleOrDefaultAsync<Group>(sql, new { id });

                return group;
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<List<BestStudentInGroupDTOs>> GetTopStudentsAsync(int groupId)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
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
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                if (group.StartDate >= group.EndDate)
                    return "StartDate must be less than EndDate";

                string sql = @"
                INSERT INTO Groups (Name, StartDate, EndDate)
                VALUES (@Name, @StartDate, @EndDate)";

                var res = await connection.ExecuteAsync(sql, group);

                return res == 0 ? "Group Not Found" : "Group Created Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> UpdateGroupAsync(int id, Group group)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
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
            using (NpgsqlConnection connection = context.GetConnection())
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

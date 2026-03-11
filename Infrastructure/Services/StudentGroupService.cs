using System;
using Dapper;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Services;

public class StudentGroupService : IStudentGroupService
{
    private readonly DataContext context = new();

    public async Task<List<Student>> GetStudentsByGroupIdAsync(int groupId)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT s.* 
                FROM Students AS s
                JOIN StudentGroups AS sg ON s.Id = sg.StudentId
                WHERE sg.GroupId = @groupId";

                var res = await connection.QueryAsync<Student>(sql, new { groupId });

                return res.ToList();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> AddStudentGroupAsync(int groupId, int studentId)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql8 = @"
                SELECT 1 FROM StudentGroups
                WHERE StudentId = @StudentId AND GroupId = @GroupId";

                var res8 = await connection.ExecuteScalarAsync<bool>(sql8, new { studentId, groupId });

                if (res8 == false)
                    return "A student cannot be in a group twice.";

                string sql = @"
                INSERT INTO StudentGroups (GroupId, StudentId)
                VALUES (@GroupId, @StudentId)";

                var res = await connection.ExecuteAsync(sql, new { groupId, studentId });

                return res == 0 ? "Student Not Found In This Group" : "Student Added to Group Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> DeleteStudentGroupAsync(int groupId, int studentId)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                DELETE FROM StudentGroups
                WHERE GroupId = @groupId AND StudentId = @studentId";

                var res = await connection.ExecuteAsync(sql, new { groupId, studentId });

                return res == 0 ? "Student Not Found In This Group" : "Student Deleted From Group Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}

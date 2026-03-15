using System;
using Dapper;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Services;

public class ProgressBookService(DataContext context) : IProgressBookService
{
    public async Task<List<ProgressBook>> GetGroupProgressBook(int groupId)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT * FROM ProgressBook
                WHERE GroupId = @groupId";

                var res = await connection.QueryAsync<ProgressBook>(sql, new { groupId });

                return res.ToList();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<List<ProgressBook>> GetStudentProgressBook(int studentId)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT * FROM ProgressBook
                WHERE StudentId = @studentId";

                var res = await connection.QueryAsync<ProgressBook>(sql, new { studentId });

                return res.ToList();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> AddProgressBookAsync(ProgressBook progressBook)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql1 = @"
                SELECT 1 FROM Students
                WHERE Id = @StudentId";

                var res1 = await connection.ExecuteScalarAsync<bool>(sql1, new { progressBook.StudentId });

                if (res1 == false)
                    return "Student not exists";

                string sql2 = @"
                SELECT 1 FROM StudentGroups
                WHERE StudentId = @StudentId AND GroupId = @GroupId";

                var res2 = await connection.ExecuteScalarAsync<bool>(sql2, new { progressBook.StudentId, progressBook.GroupId });

                if (res2 == false)
                    return "Student is not in this group";

                string sql3 = @"
                SELECT 1 FROM TimeTable
                WHERE GroupId = @GroupId";

                var res3 = await connection.ExecuteScalarAsync<bool>(sql3, new { progressBook.GroupId });

                if (res3 == false)
                    return "Group has no timetable";

                if (progressBook.Grade < 1 || progressBook.Grade > 100)
                    return "Grade must be between 1 and 100";

                if (progressBook.LateMinutes <= 0 || progressBook.LateMinutes >= 120)
                    return "LateMinutes must be between 0 and 120";

                string sql = @"
                INSERT INTO ProgressBook (Grade, StudentId, IsAttended, Date, GroupId, Notes, LateMinutes, UpdateByUserId)
                VALUES (@Grade, @StudentId, @IsAttended, @Date, @GroupId, @Notes, @LateMinutes, @UpdateByUserId)";

                var res = await connection.ExecuteAsync(sql, progressBook);

                return res == 0 ? "ProgressBook Not Created" : "ProgressBook Created Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> UpdateProgressBookAsync(int id, ProgressBook progressBook)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                UPDATE ProgressBook SET
                Grade = @Grade,
                StudentId = @StudentId,
                IsAttended = @IsAttended,
                Date = @Date,
                GroupId = @GroupId,
                Notes = @Notes,
                LateMinutes = @LateMinutes,
                UpdateByUserId = @UpdateByUserId
                WHERE Id = @id";

                var res = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    Grade = progressBook.Grade,
                    StudentId = progressBook.StudentId,
                    IsAttended = progressBook.IsAttended,
                    Date = progressBook.Date,
                    GroupId = progressBook.GroupId,
                    Notes = progressBook.Notes,
                    LateMinutes = progressBook.LateMinutes,
                    UpdateByUserId = progressBook.UpdateByUserId
                });

                return res == 0 ? "Progress Book Not Found" : "Progress Book Updated Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}

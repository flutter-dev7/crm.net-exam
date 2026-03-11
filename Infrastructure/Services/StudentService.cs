using System;
using Dapper;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Services;

public class StudentService : IStudentService
{
    private readonly DataContext context = new();

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = "SELECT * FROM Students";

                var students = await connection.QueryAsync<Student>(sql);

                return students.ToList();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT * FROM Students
                WHERE Id = @id";

                var student = await connection.QuerySingleOrDefaultAsync<Student>(sql, new { id });

                return student;
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<int> GetAverageGradeAsync(int id)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT AVG(Grade) FROM ProgressBook
                WHERE StudentId = @id";

                var res = await connection.QuerySingleOrDefaultAsync<int>(sql, new { id });

                return res;
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<int> GetAttendancePercentageAsync(int studentId)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT COUNT(*) FROM ProgressBook
                WHERE StudentId = @studentId";

                var total = await connection.ExecuteScalarAsync<int>(sql, new { studentId });

                if (total == 0)
                    return 0;

                string attendedSql = @"
                SELECT COUNT(*) FROM ProgressBook
                WHERE StudentId = @studentId AND IsAttended = TRUE";

                var res = await connection.ExecuteScalarAsync<int>(attendedSql, new { studentId });

                int percent = res / total * 100;

                return percent;
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> AddStudentAsync(Student student)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                INSERT INTO Students (FirstName, LastName, Phone, Email)
                VALUES (@FirstName, @LastName, @Phone, @Email)";

                var res = await connection.ExecuteAsync(sql, student);

                return res == 0 ? "Student Not Found" : "Student Created Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> UpdateStudentAsync(int id, Student student)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                UPDATE Students SET
                FirstName = @FirstName,
                LastName = @LastName,
                Phone = @Phone,
                Email = @Email
                WHERE Id = @id";

                var res = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Phone = student.Phone,
                    Email = student.Email
                });

                return res == 0 ? "Student Not Found" : "Student Updated Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> DeleteStudentAsync(int id)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                DELETE FROM Students
                WHERE Id = @id";

                var res = await connection.ExecuteAsync(sql, new { id });

                return res == 0 ? "Student Not Found" : "Student Deleted Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}

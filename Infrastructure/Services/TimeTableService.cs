using System;
using Dapper;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Services;

public class TimeTableService : ITimeTableService
{
    private readonly DataContext context = new();

    public async Task<List<TimeTable>> GetTimeTableByGroupIdAsync(int groupId)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                SELECT * FROM TimeTable
                WHERE GroupId = @groupId";

                var res = await connection.QueryAsync<TimeTable>(sql, new { groupId });

                return res.ToList();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> AddTimeTableAsync(TimeTable timeTable)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                if (timeTable.FromTime >= timeTable.ToTime)
                    return "FromTime must be less than ToTime";

                string sql7 = @"
                SELECT 1 FROM TimeTable
                WHERE GroupId = @GroupId AND DayOfWeek = @DayOfWeek";

                var res7 = await connection.ExecuteScalarAsync<bool>(sql7, new { timeTable.GroupId, timeTable.DayOfWeek });

                if (res7 == false)
                    return "You cannot create 2 schedules on the same day";

                string sql = @"
                INSERT INTO TimeTable (DayOfWeek, FromTime, ToTime, GroupId)
                VALUES (@DayOfWeek, @FromTime, @ToTime, @GroupId)";

                var res = await connection.ExecuteAsync(sql, new
                {
                    DayOfWeek = timeTable.DayOfWeek,
                    FromTime = timeTable.FromTime.ToTimeSpan(),
                    ToTime = timeTable.ToTime.ToTimeSpan(),
                    GroupId = timeTable.GroupId
                });
                return res == 0 ? "TimeTable Not Created" : "TimeTable Created Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> UpdateTimeTableAsync(int id, TimeTable timeTable)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                UPDATE TimeTable SET
                DayOfWeek = @DayOfWeek,
                FromTime = @FromTime,
                ToTime = @ToTime,
                GroupId = @GroupId
                WHERE Id = @id";

                var res = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    DayOfWeek = timeTable.DayOfWeek,
                    FromTime = timeTable.FromTime.ToTimeSpan(),
                    ToTime = timeTable.ToTime.ToTimeSpan(),
                    GroupId = timeTable.GroupId
                });

                return res == 0 ? "TimeTable Not Found" : "TimeTable Updated Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<string> DeleteTimeTableAsync(int id)
    {
        try
        {
            using (NpgsqlConnection connection = context.GetConnection())
            {
                connection.Open();

                string sql = @"
                DELETE FROM TimeTable
                WHERE Id = @id";

                var res = await connection.ExecuteAsync(sql, new { id });

                return res == 0 ? "TimeTable Not Found" : "TimeTable Deleted Successfully";
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}

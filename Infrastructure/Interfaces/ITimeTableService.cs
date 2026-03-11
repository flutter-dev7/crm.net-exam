using System;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ITimeTableService
{
    Task<List<TimeTable>> GetTimeTableByGroupIdAsync(int groupId);
    Task<string> AddTimeTableAsync(TimeTable timeTable);
    Task<string> UpdateTimeTableAsync(int id, TimeTable timeTable);
    Task<string> DeleteTimeTableAsync(int id);
}

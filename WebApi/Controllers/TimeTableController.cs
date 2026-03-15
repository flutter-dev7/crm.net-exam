using System;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/")]
public class TimeTableController(ITimeTableService service)
{
    [HttpGet("groups/{groupId:int}/timetable")]
    public async Task<List<TimeTable>> GetTimeTableByGroupIdAsync(int groupId)
    {
        return await service.GetTimeTableByGroupIdAsync(groupId);
    }

    [HttpPost("timetable")]
    public async Task<string> AddTimeTableAsync(TimeTable timeTable)
    {
        return await service.AddTimeTableAsync(timeTable);
    }

    [HttpPut("timetable/{id:int}")]
    public async Task<string> UpdateTimeTableAsync(int id, TimeTable timeTable)
    {
        return await service.UpdateTimeTableAsync(id, timeTable);
    }

    [HttpDelete("timetable/{id:int}")]
    public async Task<string> DeleteTimeTableAsync(int id)
    {
        return await service.DeleteTimeTableAsync(id);
    }
}

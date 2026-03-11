using System;

namespace Domain.Entities;

public enum DayOfWeekEnum
{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7
}

public class TimeTable
{
    public int Id { get; set; }
    public DayOfWeekEnum DayOfWeek { get; set; }
    public TimeOnly FromTime { get; set; }
    public TimeOnly ToTime { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int GroupId { get; set; }
}

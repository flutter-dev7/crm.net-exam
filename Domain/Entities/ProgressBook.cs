using System;

namespace Domain.Entities;

public class ProgressBook
{
    public int Id { get; set; }
    public int Grade { get; set; }
    public int StudentId { get; set; }
    public bool IsAttended { get; set; }
    public DateTime Date { get; set; }
    public int GroupId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int LateMinutes { get; set; }
    public string UpdateByUserId { get; set; } = string.Empty;
}

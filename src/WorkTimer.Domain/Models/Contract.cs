using System;
using System.Collections.Generic;

namespace WorkTimer.Domain.Models;

public class Contract
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Employer { get; set; }
    public int HoursPerWeek { get; set; }
    public bool IsCurrent { get; set; }
    public int UserId { get; set; }
    public TimeSpan TotalOverhours { get; set; }

    public ICollection<WorkDay>? WorkDays { get; set; }
    public ICollection<Todo>? Todos { get; set; }
    public ICollection<Note>? Notes { get; set; }
    public AppUser? User { get; set; }
}
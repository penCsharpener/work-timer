﻿using System;
using WorkTimer.Domain.Models.Enums;

namespace WorkTimer.Domain.Models;

public sealed class Todo
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser AppUser { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public DateTime? Deadline { get; set; } = default!;
    public TodoPriority Priority { get; set; }
    public int ContractId { get; set; }
    public Contract Contract { get; set; } = default!;
    public bool IsContractIndependent { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}

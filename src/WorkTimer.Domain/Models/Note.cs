using System;

namespace WorkTimer.Domain.Models;
public sealed class Note
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Tags { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime? ReminderAt { get; set; }
    public AppUser AppUser { get; set; } = default!;
    public int ContractId { get; set; }
    public Contract Contract { get; set; } = default!;
    public bool IsContractIndependent { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}

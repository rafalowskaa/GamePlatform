using GamePlatform.Shared.Enums;
using System;

namespace GamePlatform.Shared.Models;

public class UserReport
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReporterId { get; set; }
    public User? Reporter { get; set; }

    public Guid ReportedUserId { get; set; }
    public User? ReportedUser { get; set; }

    public string Category { get; set; } = "";   // spam, nadużycie itd.
    public string Description { get; set; } = "";
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public ReportStatus Status { get; set; } = ReportStatus.Pending;
    public string? StaffNotes { get; set; }
}

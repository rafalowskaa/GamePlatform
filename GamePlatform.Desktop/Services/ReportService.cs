using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public sealed class ReportService : IReportService
{
    private readonly AppDbContext _db;
    public ReportService(AppDbContext db) => _db = db;

    public async Task SubmitAsync(Guid from, Guid to, string cat, string desc)
    {
        _db.UserReports.Add(new UserReport
        {
            ReporterId = from,
            ReportedUserId = to,
            Category = cat,
            Description = desc
        });
        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<UserReport>> GetAllAsync() =>
        await _db.UserReports
                 .Include(r => r.Reporter)
                 .Include(r => r.ReportedUser)
                 .AsNoTracking()
                 .ToListAsync();

    public async Task UpdateStatusAsync(Guid id, ReportStatus st, string? notes)
    {
        var r = await _db.UserReports.FindAsync(id);
        if (r is null) return;
        r.Status = st;
        r.StaffNotes = notes;
        await _db.SaveChangesAsync();
    }

    public async Task SendWarningAsync(Guid reportId)
    {
        var r = await _db.UserReports.FindAsync(reportId);
        if (r is null) return;
        r.Status = ReportStatus.Closed;
        r.StaffNotes = "Warning";
        await _db.SaveChangesAsync();
    }

    public async Task CancelAsync(Guid reportId)
    {
        var r = await _db.UserReports.FindAsync(reportId);
        if (r is null) return;
        r.Status = ReportStatus.Closed;
        r.StaffNotes = "Cancelled";
        await _db.SaveChangesAsync();
    }

    public async Task BlockReportedUserAsync(Guid reportId)
    {
        var r = await _db.UserReports.Include(u => u.ReportedUser)
                                     .FirstOrDefaultAsync(u => u.Id == reportId);
        if (r is null || r.ReportedUser is null) return;

        r.ReportedUser.BlockedUntilUtc = DateTime.UtcNow.AddDays(7);
        r.ReportedUser.BlockReason = "Report block";
        r.Status = ReportStatus.Closed;
        await _db.SaveChangesAsync();
    }
}

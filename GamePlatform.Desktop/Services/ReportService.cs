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
    private readonly IChatService _chat;
    public ReportService(AppDbContext db, IChatService chat)
    {
        _db = db;
        _chat = chat;
    }

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

        var msg = new ChatMessage(Guid.Empty, r.ReportedUserId,
           "Otrzymałeś ostrzeżenie od administracji z powodu zgłoszenia, " +
           "kolejny raz będzie powodem do zablokowania konta na tydzień bez możliwości odwołania");
        await _chat.SendAsync(msg);
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

    public async Task DeleteAsync(Guid reportId)
    {
        var r = await _db.UserReports.FindAsync(reportId);
        if (r is null) return;
        _db.UserReports.Remove(r);
        await _db.SaveChangesAsync();
    }
}

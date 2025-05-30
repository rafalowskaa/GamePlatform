using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public interface IReportService
{
    Task SubmitAsync(Guid reporterId, Guid targetId,
                     string category, string desc);
    Task<IReadOnlyList<UserReport>> GetAllAsync();
    Task UpdateStatusAsync(Guid id, ReportStatus st, string? notes);

    Task SendWarningAsync(Guid reportId);
    Task CancelAsync(Guid reportId);
    Task BlockReportedUserAsync(Guid reportId);
}

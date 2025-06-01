using System;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public interface IRankService
{
    Task AddXpAsync(Guid userId, int xp);
}
using EcommerceInventory.Application.DTOs.Auth;

namespace EcommerceInventory.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string? Token, string? Error)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginDto dto);
    }
}

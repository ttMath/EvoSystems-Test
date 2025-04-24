using EvoSystems.Dto;

namespace EvoSystems.Authentication.Interfaces
{
    public interface ITokenManager
    {
        string GenerateToken(EmployeeDetailDto employee);
        string GenerateRefreshToken(EmployeeDetailDto employee);
        Task<(bool isValid, string? RG)> ValidateTokenAsync(string token);
    }
}

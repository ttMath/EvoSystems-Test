using EvoSystems.Authentication.Interfaces;
using EvoSystems.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EvoSystems.Authentication
{
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration _configuration;

        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(EmployeeDetailDto employee)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(jwtSettings["SecretKey"] ?? string.Empty));

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, employee.RG),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var expirationTimeInMinutes = jwtSettings.GetValue<int>("ExpirationTimeInMinutes");

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetValue<string>("Issuer"),
                audience: jwtSettings.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(5),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(EmployeeDetailDto employee)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(jwtSettings["SecretKey"] ?? string.Empty));

            var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, employee.RG),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var expirationTimeInMinutes = jwtSettings.GetValue<int>("RefreshExpirationTimeInMinutes");

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetValue<string>("Issuer"),
                audience: jwtSettings.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationTimeInMinutes),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(bool isValid, string? RG)> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return (false, null);

            var tokenParameters = TokenHelpers.GetTokenValidationParameters(_configuration);
            var validTokenResult = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, tokenParameters);

            if (!validTokenResult.IsValid)
                return (false, null);

            var employeeRG = validTokenResult
                .Claims.FirstOrDefault(c => c.Key == ClaimTypes.NameIdentifier).Value as string;

            return (true, employeeRG);
        }
    }
}

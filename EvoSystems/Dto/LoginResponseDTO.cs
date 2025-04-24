namespace EvoSystems.Dto
{
    public class LoginResponseDTO(string token, string refreshToken)
    {
        public string Token { get; set; } = token;
        public string RefreshToken { get; set; } = refreshToken;
    }
}

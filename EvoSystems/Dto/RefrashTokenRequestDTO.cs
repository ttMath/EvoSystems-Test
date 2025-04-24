namespace EvoSystems.Dto
{
    public class RefrashTokenRequestDTO(string refreshToken)
    {
        public string RefreshToken { get; set; } = refreshToken;
    }
}

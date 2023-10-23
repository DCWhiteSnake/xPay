namespace xPayServer.Models
{
    public class AuthenticatedResponseModel
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}

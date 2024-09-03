namespace TodoAPI.Config;

internal static class SecurityConfig
{
    internal const string ClaimSubject = "sub";

    internal const string ClaimEmail = "email";

    internal const string AuthenticationType = "Bearer";

    internal static byte[] GetTokenSecret(IConfiguration configuration)
    {
        return Convert.FromBase64String(configuration["Authentication:SecretKey"] ??
            throw new ArgumentNullException("Could not load secret for token authentication"));

    }

    internal static string GetTokenIssuer(IConfiguration configuration)
    {
        return configuration["Authentication:Issuer"] ??
            throw new ArgumentNullException("Could not load issuer for token authentication");

    }

    internal static string GetTokenAudience(IConfiguration configuration)
    {
        return configuration["Authentication:Audience"] ??
            throw new ArgumentNullException("Could not load audience for token authentication");

    }
}

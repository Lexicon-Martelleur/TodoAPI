namespace TodoAPI.Constants;

internal static class ApplicationConfiguration
{
    internal static readonly (string Dev, string Prod) CorsPolicies = (
        Dev: "DevCorsPolicy",
        Prod: "ProdCorsPolicy"
    );
}

namespace TodoAPI.Constants;

internal class Router
{
    private const string BaseRoute = "api/v{version:apiVersion}";
    
    internal const string Authenticate = BaseRoute + "/authenticate";

    internal const string Todo = BaseRoute + "/todo";
}

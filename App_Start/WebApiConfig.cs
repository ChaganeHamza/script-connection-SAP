using System.Web.Http;
using System.Web.Http.Cors;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Configuration et services API Web

        // Enable CORS
        var cors = new EnableCorsAttribute("*", "*", "*"); // Allow all origins, headers, and methods
        config.EnableCors(cors);

        // Configuration des routes de l'API Web
        config.MapHttpAttributeRoutes();
    }
}
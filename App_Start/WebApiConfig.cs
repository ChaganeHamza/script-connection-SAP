using System.Web.Http;
using System.Web.Http.Cors;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Configuration et services API Webgit 

        // Enable CORS
        var cors = new EnableCorsAttribute("https://localhost:44330", "https://localhost:3000", "*"); // Allow all origins, headers, and methods
        config.EnableCors(cors);

        // Configuration des routes de l'API Web
        config.MapHttpAttributeRoutes();
    }
}
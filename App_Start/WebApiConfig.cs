using System.Web.Http;
using System.Web.Http.Cors;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Configuration et services Web API
        // Activer CORS
        var cors = new EnableCorsAttribute("http://localhost:3000", "https://localhost:44330", "*");
        config.EnableCors(cors);

        // Itinéraires de l'API Web
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}

using System.Web.Mvc;
using System.Web.Routing;

public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        // Новый маршрут для передачи параметра id через точку
        routes.MapRoute(
            name: "ApiError",
            url: "api/error/{code}/{id}.{format}",
            defaults: new { controller = "Error", action = "Error", id = UrlParameter.Optional }
        );

        // Основной маршрут по умолчанию
        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
    }
}

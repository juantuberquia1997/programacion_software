using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace apiSusCopias
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de Web API
            // Rutas de Web API
            //[EnableCors(origins: "", headers: "*", methods: "*")];

            // Configuración y servicios de Web API
            /*var cors = new EnableCorsAttribute(
                origins: "http://localhost:3000", // pon aquí tu origen real, no vacío
                headers: "*",
                methods: "*"
            );
            */

            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

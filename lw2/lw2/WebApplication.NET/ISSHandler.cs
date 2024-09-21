using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace WebApplication.NET
{
    public class ISSHandler : IHttpHandler
    {
        private static Stack<int> globalStack = new Stack<int>(); // Глобальный стек
        private static int result = 0; // Первоначальное значение RESULT

        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            string requestType = context.Request.HttpMethod;

            // Обработка CORS preflight-запросов
            if (requestType == "OPTIONS")
            {
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                context.Response.StatusCode = 200;
                context.Response.End();
                return;
            }

            switch (requestType)
            {
                case "GET": HandleGetRequest(context); break;
                case "POST": HandlePostRequest(context); break;
                case "PUT": HandlePutRequest(context); break;
                case "DELETE": HandleDeleteRequest(context); break;
                default:
                    context.Response.StatusCode = 405; // Метод не разрешен
                    break;
            }
        }

        private void HandleGetRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new { RESULT = result };
            context.Response.Write(new JavaScriptSerializer().Serialize(response));
        }

        private void HandlePostRequest(HttpContext context)
        {
            try
            {
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                var data = new JavaScriptSerializer().Deserialize<Dictionary<string, int>>(json);

                if (data != null && data.ContainsKey("RESULT"))
                {
                    result = data["RESULT"];
                    context.Response.ContentType = "application/json";
                    var response = new { RESULT = result };
                    context.Response.Write(new JavaScriptSerializer().Serialize(response));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write("Invalid or missing RESULT parameter.");
                }
            }
            catch
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Invalid request body.");
            }
        }

        private void HandlePutRequest(HttpContext context)
        {
            try
            {
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                var data = new JavaScriptSerializer().Deserialize<Dictionary<string, int>>(json);

                if (data != null && data.ContainsKey("ADD"))
                {
                    int add = data["ADD"];
                    globalStack.Push(add);
                    result += add; // Обновляем RESULT после добавления
                    context.Response.ContentType = "application/json";
                    var response = new { RESULT = result };
                    context.Response.Write(new JavaScriptSerializer().Serialize(response));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write("Invalid or missing ADD parameter.");
                }
            }
            catch
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Invalid request body.");
            }
        }

        private void HandleDeleteRequest(HttpContext context)
        {
            if (globalStack.Count > 0)
            {
                int poppedValue = globalStack.Pop();
                result -= poppedValue; // Обновляем RESULT после удаления
                context.Response.ContentType = "application/json";
                var response = new { RESULT = result };
                context.Response.Write(new JavaScriptSerializer().Serialize(response));
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Stack is empty, nothing to pop.");
            }
        }
    }
}

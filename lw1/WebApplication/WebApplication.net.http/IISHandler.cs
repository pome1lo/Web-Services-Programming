using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace WebApplication.net.http
{
    public class IISHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var requestType = context.Request.HttpMethod;
            switch (requestType)
            {
                case "GET": HandleGetRequest(context); break;
                case "PUT": HandlePutRequest(context); break;
                case "POST": HandlePostRequest(context); break;
                case "DELETE": HandleDeleteRequest(context); break;
                default: context.Response.StatusCode = 405; break;
            }
        }

        private void HandleGetRequest(HttpContext context)
        {
            int result = (int?)context.Session["RESULT"] ?? 0;

            context.Response.ContentType = "application/json";
            context.Response.Write("{\"RESULT\": " + result + "}");
        }

        private void HandlePostRequest(HttpContext context)
        {
            if (int.TryParse(context.Request.Params["RESULT"], out int result))
            {
                context.Session["RESULT"] = result;
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"RESULT\": " + result + "}");
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private void HandlePutRequest(HttpContext context)
        {
            if (int.TryParse(context.Request.Params["ADD"], out int addValue))
            {
                var stack = (Stack<int>)context.Session["Stack"] ?? new Stack<int>();
                stack.Push(addValue);
                context.Session["Stack"] = stack;

                int result = (int?)context.Session["RESULT"] ?? 0;
                result += addValue;
                context.Session["RESULT"] = result;

                context.Response.ContentType = "application/json";
                context.Response.Write("{\"RESULT\": " + result + "}");
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private void HandleDeleteRequest(HttpContext context)
        {
            var stack = (Stack<int>)context.Session["Stack"];
            if (stack != null && stack.Count > 0)
            {
                int poppedValue = stack.Pop();
                context.Session["Stack"] = stack;

                int result = (int?)context.Session["RESULT"] ?? 0;
                result -= poppedValue;
                context.Session["RESULT"] = result;

                context.Response.ContentType = "application/json";
                context.Response.Write("{\"RESULT\": " + result + "}");
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Stack is empty, nothing to pop.");
            }
        }
    }
}

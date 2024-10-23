using Simplex.Models;
using System;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;

// http://localhost:49541/Simplex.asmx

namespace Simplex
{
    [WebService(Namespace = "http://paa/", Description = "Simplex")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [System.Web.Script.Services.ScriptService]
    public class Simplex : System.Web.Services.WebService
    {
        [WebMethod(MessageName = "Add", Description = "Возвращает значение суммы двух параметров")]
        public int Add(int x, int y) => x + y;

        [WebMethod(MessageName = "Concat", Description = "Возвращает конкатенацию первого и второго параметров ")]
        public string Concat(string s, string d) => s + d;


        [WebMethod(MessageName = "Sum", Description = "Возвращает объект A")]
        public A Sum(A a1, A a2)
        {
            System.IO.Stream requestStream = HttpContext.Current.Request.InputStream;
            requestStream.Position = 0;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(requestStream))
            {
                string requestBody = reader.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(requestBody);
                Console.WriteLine(requestBody);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(requestBody);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("paa", "http://paa/");

                XmlNode aNode = doc.SelectSingleNode("//paa:Sum/paa:a", nsmgr);
                XmlNode bNode = doc.SelectSingleNode("//paa:Sum/paa:b", nsmgr);

                if (aNode != null)
                {
                    a1 = new A(
                        aNode["s"]?.InnerText,
                        int.Parse(aNode["k"]?.InnerText ?? "0"),
                        float.Parse(aNode["f"]?.InnerText ?? "0")
                    );
                }

                if (bNode != null)
                {
                    a2 = new A(
                        bNode["s"]?.InnerText,
                        int.Parse(bNode["k"]?.InnerText ?? "0"),
                        float.Parse(bNode["f"]?.InnerText ?? "0")
                    );
                }

            }

            return new A(a1.s + a2.s, a1.k + a2.k, a1.f + a2.f);
        }

        [WebMethod(MessageName = "HelloWorkd")]
        public int HelloWorld(int x, int y) => x + y;



        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod(MessageName = "POSTMAN_1")]
        public int POSTMAN_1(int a, int b) => a + b;

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod(MessageName = "POSTMAN_2")]
        public int POSTMAN_2(int a, int b) => a - b;


        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod(MessageName = "Adds")]
        public int Adds(int a, int b) => a + b;
    }
}

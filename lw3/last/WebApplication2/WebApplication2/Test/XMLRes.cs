using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http;
using System.Xml.Linq;

namespace WebApplication2.Test
{
    internal class XMLRes
    {
        public class XmlResult : IHttpActionResult
        {
            private readonly XDocument _xml;

            public XmlResult(XDocument xml)
            {
                _xml = xml;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_xml.ToString(), Encoding.UTF8, "application/xml")
                };
                return Task.FromResult(response);
            }
        }

    }
}
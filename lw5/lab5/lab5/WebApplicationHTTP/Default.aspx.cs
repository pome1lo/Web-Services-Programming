using System;
using System.Web.UI;
using WebApplicationHTTP.ServiceReference1;

namespace WebApplicationHTTP
{
    // svcutil.exe http://..
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string resultHTML = "";

            var a = new A();
            var b = new A();

            a.f = float.Parse("1,1");
            b.f = float.Parse("2,2");

            a.k = 3;
            b.k = 4;

            a.s = "hello ";
            b.s = "world";

            var client = new WCFSimplexClient();

            Add.Text += client.Add(22, 33);

            Concat.Text += client.Concat("вася", 2003);

            var resultA = client.Sum(a, b);

            resultHTML += $"f: {resultA.f}\nk: {resultA.k}\ns: {resultA.s}";

            result_F.Text = resultA.f.ToString();
            result_K.Text = resultA.k.ToString();
            result_S.Text = resultA.s.ToString();


            //result.Text = resultHTML;

            client.Close();
        }
    }
}
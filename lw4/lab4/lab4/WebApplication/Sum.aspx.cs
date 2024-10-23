using System;
namespace WebApplication
{
    public partial class Sum : System.Web.UI.Page
    {
        private Simplex _client;

        protected void Page_Load(object sender, EventArgs e)
        {
            _client = new Simplex();
        }

        protected void SumClick(object sender, EventArgs e)
        {
            var a1 = new A { s = S1.Text, k = Convert.ToInt32(K1.Text), f = float.Parse(F1.Text) };
            var a2 = new A { s = S2.Text, k = Convert.ToInt32(K2.Text), f = float.Parse(F2.Text) };
            Console.WriteLine(a1);
            Console.WriteLine(a2);
            var res = _client.Sum(a1, a2);

            S3.Text = res.s;
            K3.Text = res.k.ToString();
            F3.Text = res.f.ToString();
        }
    }
}
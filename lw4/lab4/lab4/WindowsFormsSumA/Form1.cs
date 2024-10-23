using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsSumA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s1 = S1.Text;
            string s2 = S2.Text;
            string k1 = K1.Text;
            string k2 = K2.Text;
            string f1 = F1.Text;
            string f2 = F2.Text;

            string url = "http://localhost:49541/Simplex.asmx"; // CHANGE PORT

            string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                      <soap:Body>
                                        <Sum xmlns=""http://paa/"">
                                          <a>
                                            <s>{s1}</s>
                                            <k>{k1}</k>
                                            <f>{f1}</f>
                                          </a>
                                          <b>
                                            <s>{s2}</s>
                                            <k>{k2}</k>
                                            <f>{f2}</f>
                                          </b>
                                        </Sum>
                                      </soap:Body>
                                    </soap:Envelope>";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "text/xml; charset=utf-8";
            request.Accept = "text/xml";
            request.Method = "POST";
            using (Stream stream = request.GetRequestStream())
            {
                byte[] data = Encoding.UTF8.GetBytes(soapRequest);
                stream.Write(data, 0, data.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    ParseAndDisplayResult(result);
                }
            }
        }

        private void ParseAndDisplayResult(string soapResponse)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(soapResponse);

            string s3 = doc.GetElementsByTagName("s")[0].InnerText;
            string k3 = doc.GetElementsByTagName("k")[0].InnerText;
            string f3 = doc.GetElementsByTagName("f")[0].InnerText;

            S3.Text = s3;
            K3.Text = k3;
            F3.Text = f3;
        }

        private void K1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

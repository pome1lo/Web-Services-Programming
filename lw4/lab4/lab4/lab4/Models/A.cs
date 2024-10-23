using System.Xml.Serialization;

namespace Simplex.Models
{
    public class A
    {
        [XmlElement(ElementName = "s")] public string s;
        [XmlElement(ElementName = "k")] public int k;
        [XmlElement(ElementName = "f")] public float f;

        public A() { }

        public A(string v1, int v2, float v3)
        {
            s = v1;
            k = v2;
            f = v3;
        }
    }
}
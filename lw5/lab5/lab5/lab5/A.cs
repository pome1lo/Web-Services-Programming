using System.Runtime.Serialization;

namespace lab5
{
    [DataContract]
    public class A
    {
        [DataMember] public string s;
        [DataMember] public int k;
        [DataMember] public float f;

        public static A operator +(A a, A b) => new A(a.s + b.s, a.k + b.k, a.f + b.f);

        public A() { }
        public A(string s, int k, float f)
        {
            this.s = s;
            this.k = k;
            this.f = f;
        }
    }
}

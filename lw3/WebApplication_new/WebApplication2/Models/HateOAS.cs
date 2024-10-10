namespace lw3.Models
{
    public class HateOAS
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }

        public HateOAS(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
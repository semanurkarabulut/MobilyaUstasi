namespace MobilyaUstasi.Models
{
    public class SliderModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string url { get; set; }
        public string img { get; set; }
        public IFormFile file { get; set; }
    }
}

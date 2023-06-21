namespace MobilyaUstasi.Models
{
    public class MobilyaModel
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string Image { get; set; }
        public IFormFile file { get; set; }
    }
}

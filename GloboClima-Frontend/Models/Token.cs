namespace GloboClima_Frontend.Models
{
    public class Token
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string Key { get; set; } = null!;
        public DateTime ValidTo { get; set; }
    }
}

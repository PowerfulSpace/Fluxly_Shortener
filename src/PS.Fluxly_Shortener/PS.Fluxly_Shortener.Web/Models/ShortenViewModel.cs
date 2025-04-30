using System.ComponentModel.DataAnnotations;

namespace PS.Fluxly_Shortener.Web.Models
{
    public class ShortenViewModel
    {
        [Required(ErrorMessage = "Введите ссылку")]
        [Url(ErrorMessage = "Неверный формат ссылки")]
        public string LongUrl { get; set; } = string.Empty;

        public string? ShortUrl { get; set; }
    }
}

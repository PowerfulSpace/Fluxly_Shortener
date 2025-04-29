using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PS.Fluxly_Shortener.API.Configuration;
using PS.Fluxly_Shortener.API.Models;
using PS.Fluxly_Shortener.API.Services.Interfaces;

namespace PS.Fluxly_Shortener.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShortenerController : ControllerBase
    {
        private readonly ILogger<ShortenerController> _logger;
        private readonly ILinkShortenerService _linkShortener;
        private readonly string _baseUrl;


        public ShortenerController(
            ILogger<ShortenerController> logger,
            ILinkShortenerService linkShortener,
            IOptions<ShortenerSettings> options
            )
        {
            _logger = logger;
            _linkShortener = linkShortener;
            _baseUrl = options.Value.BaseUrl.TrimEnd('/');
        }


        [HttpPost("shorten")]
        public IActionResult Shorten([FromBody] LinkRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.LongUrl))
                return BadRequest("Long URL is required.");

            var shortUrl = _linkShortener.Shorten(request.LongUrl);

            return Ok(new LinkResponse { ShortUrl = shortUrl });
        }

        [HttpGet("expand")]
        public IActionResult Expand([FromQuery] string shortUrl)
        {
            var longUrl = _linkShortener.Expand(shortUrl);

            if (longUrl == null)
                return NotFound("Short URL not found.");

            return Ok(new { LongUrl = longUrl });
        }


        [HttpGet("/r/{code}")]
        public IActionResult RedirectToOriginal(string code)
        {
            var fullShortUrl = $"{_baseUrl}/r/{code}";
            var longUrl = _linkShortener.Expand(fullShortUrl);

            if (longUrl == null)
                return NotFound("Link not found.");

            return Redirect(longUrl);
        }
    }
}

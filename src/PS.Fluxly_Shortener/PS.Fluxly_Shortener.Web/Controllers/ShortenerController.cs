using Microsoft.AspNetCore.Mvc;
using PS.Fluxly_Shortener.Web.Models;

namespace PS.Fluxly_Shortener.Web.Controllers
{
    public class ShortenerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ShortenerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ShortenViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ShortenViewModel model)
        {
            var client = _httpClientFactory.CreateClient("ShortenerApi");

            var response = await client.PostAsJsonAsync("shortener/shorten", new { LongUrl = model.LongUrl });

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<LinkResponse>();
                model.ShortUrl = data?.ShortUrl;
            }
            else
            {
                ModelState.AddModelError("", "Ошибка при сокращении ссылки");
            }

            return View(model);
        }
    }
}


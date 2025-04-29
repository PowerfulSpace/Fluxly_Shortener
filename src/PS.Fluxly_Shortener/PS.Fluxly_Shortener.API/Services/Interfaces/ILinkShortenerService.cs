namespace PS.Fluxly_Shortener.API.Services.Interfaces
{
    public interface ILinkShortenerService
    {
        string Shorten(string longUrl);

        string? Expand(string shortUrl);
    }
}

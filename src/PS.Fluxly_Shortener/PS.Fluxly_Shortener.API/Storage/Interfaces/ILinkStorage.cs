namespace PS.Fluxly_Shortener.API.Storage.Interfaces
{
    public interface ILinkStorage
    {
        void Save(string shortUrl, string longUrl);
        bool TryGet(string shortUrl, out string longUrl);
    }
}

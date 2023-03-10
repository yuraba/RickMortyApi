using Microsoft.Extensions.Caching.Memory;
using RickAndMorty.Services;
using upswotProj.Models;

namespace RickAndMorty.Cache;

public class RequestCache : IRequestCache
{
    private  readonly IMemoryCache cache;
    private readonly IRickAndMortyService _rickAndMortyService;
    
    public RequestCache(IMemoryCache cache, IRickAndMortyService _rickAndMortyService)
    {
        this.cache = cache;
        this._rickAndMortyService = _rickAndMortyService;
    }

    public async Task<bool> GetData(string personName, string episodeName)
    {
        var key = personName + episodeName;
        if (!cache.TryGetValue(key, out bool response))
        {
            response = await _rickAndMortyService.checkPersonInEpisode(personName, episodeName);
            cache.Set(key, response, DateTimeOffset.Now.AddDays(7));
        }
        return response;
    }
    public async Task<Person> GetData(string personName)
    {
        var key = personName;
        if (!cache.TryGetValue(key, out Person response))
        {
            response = await _rickAndMortyService.GetPersonByName(personName, true);
            cache.Set(key, response, DateTimeOffset.Now.AddDays(7));
        }
        return response;
    }
}
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RickAndMorty.Cache;
using RickAndMorty.Services;
using upswotProj.Models;

namespace RickAndMorty.Controllers;

public class RickAndMortyController : Controller
{
    private readonly IRickAndMortyService _rickAndMortyService;
    private readonly IRequestCache _requestCache;

    public RickAndMortyController(IRickAndMortyService _rickAndMortyService, IRequestCache _requestCache)
    {
        this._rickAndMortyService = _rickAndMortyService;
        this._requestCache = _requestCache;
    }
    [HttpPost]
    [Route("/api/v1/check-person")]
    public async Task<IActionResult> CheckByNameAndEpisode(string personName, string episodeName)
    {
        var result = await _requestCache.GetData(personName, episodeName);
        if (result==CheckPersonResult.NotFound)
        {
            return NotFound();
        }
        var toReturn = result.ToString();
        return Content(toReturn == "True" ? "true" : "false");
    }

    [HttpGet]
    [Route("/api/v1/person")]
    public async Task<IActionResult> GetByName(string name)
    {
        var result = await _requestCache.GetData(name);
        if (result==null)
        {
            return NotFound();
        }
        var toReturn = JsonConvert.SerializeObject(result);
        return Content(toReturn);
    }
}

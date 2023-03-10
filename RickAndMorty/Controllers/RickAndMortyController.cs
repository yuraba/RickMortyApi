using Microsoft.AspNetCore.Mvc;
using RickAndMorty.Services;
using upswotProj.Models;

namespace RickAndMorty.Controllers;

public class RickAndMortyController : Controller
{
    private readonly IRickAndMortyService _rickAndMortyService;

    public RickAndMortyController(IRickAndMortyService _rickAndMortyService)
    {
        this._rickAndMortyService = _rickAndMortyService;
    }
    [HttpPost]
    [Route("/api/v1/check-person")]
    public async Task<IActionResult> CheckByNameAndEpisode(string personName, string episodeName)
    {
        var result = await _rickAndMortyService.checkPersonInEpisode(personName,episodeName);
            
        return Content(result ? "true" : "false");
    }

    [HttpGet]
    [Route("/api/v1/person")]
    public async Task<Person> GetByName(string name)
    {
        var person = await _rickAndMortyService.GetPersonByName(name,true);
        //Return Persong obj
        return person;
    }
}

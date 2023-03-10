using upswotProj.Models;

namespace RickAndMorty.Services;

public interface IRickAndMortyService
{
    public Task<CheckPersonResult> checkPersonInEpisode(string personName,string episodeName);
    public Task<Person> GetPersonByName(string personName, bool includeOrigin);
}
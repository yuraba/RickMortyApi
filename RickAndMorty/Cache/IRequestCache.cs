using upswotProj.Models;

namespace RickAndMorty.Cache;

public interface IRequestCache
{
    public  Task<CheckPersonResult> GetData(string personName, string episodeName);
    public  Task<Person> GetData(string personName);
}
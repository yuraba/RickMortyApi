using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using upswotProj.Models;

namespace RickAndMorty.Services;

public class RickAndMortyService: IRickAndMortyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RickAndMortyService(IHttpClientFactory _httpClientFactory)
        {
            this._httpClientFactory = _httpClientFactory;
        }
        

        public async Task<bool> checkPersonInEpisode(string personName, string episodeName)
        {
            var person = await GetPersonByName(personName);
            var episodeId = await GetEpisodeIdByName(episodeName);
            var episodeUrl = $"https://rickandmortyapi.com/api/episode/{episodeId}";
            return person.episode.Contains(episodeUrl);
        }
        
       
        
    

        public   async Task<Person> GetPersonByName(string personName,bool includeOrigin=false)
        {
            var builder = new UriBuilder("https://rickandmortyapi.com/api/character");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["name"] = personName;

            builder.Query = query.ToString();
            var url = builder.ToString();
            
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);

            var str = await response.Content.ReadAsStringAsync();
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(str)!;
            
            var results = (JArray)dynamicObject["results"];
            
            var locationBuilder = new UriBuilder(results[0]["origin"]["url"].ToString());
            var locationUrl = locationBuilder.ToString();
            HttpResponseMessage locationResponse = await client.GetAsync(locationUrl).ConfigureAwait(false);
            var locationStr = await locationResponse.Content.ReadAsStringAsync();
            var locationDynamicObject = JsonConvert.DeserializeObject<dynamic>(locationStr)!;
            var locationResults = locationDynamicObject;
            
            var toReturn = new Person
            {
                name = results[0]["name"]!.ToString(),
                status = results[0]["status"]!.ToString(),
                species = results[0]["species"]!.ToString(),
                type = results[0]["type"]!.ToString(),
                gender = results[0]["gender"].ToString(),
                origin = new Origin()
                {
                    name = results[0]["origin"]["name"]!.ToString(),
                    type = locationResults["type"].ToString(),
                    dimension = locationResults["dimension"].ToString(),
                },
            };
            return toReturn;
        }

        private async Task<string> GetEpisodeIdByName(string episodeName)
        {
            var builder = new UriBuilder("https://rickandmortyapi.com/api/episode");
            var query = HttpUtility.ParseQueryString(builder.Query);

            query["name"] = episodeName;

            builder.Query = query.ToString();
            var url = builder.ToString();
            
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
            var str = await response.Content.ReadAsStringAsync();
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(str)!;
            var episodeCode = dynamicObject["results"][0]["id"].ToString();
            return Convert.ToString(episodeCode);
        }
    }

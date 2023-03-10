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
            if (episodeId=="Episode Not Found" || person == null)
            {
                return false;
            }
            var episodeUrl = $"https://rickandmortyapi.com/api/episode/{episodeId}";
            return person.episode.Contains(episodeUrl);
        }

        private async Task<Origin> GetDimensionAndType(string originUrl)
        {
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(originUrl).ConfigureAwait(false);
            var str = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<dynamic>(str)!;
           
            var toReturn = new Origin()
                {
                    name = results["name"]!.ToString(),
                    type = results["type"]!.ToString(),
                    dimension = results["dimension"]!.ToString(),
                };
            return toReturn;
        }


        public async Task<Person> GetPersonByName(string personName,bool includeOrigin=false)
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
            if (dynamicObject["error"] == null)
            {
                var results = (JArray)dynamicObject["results"];
                var origin = await GetDimensionAndType(results[0]["origin"]["url"].ToString());

                var toReturn = new Person
                {
                    name = results[0]["name"]!.ToString(),
                    status = results[0]["status"]!.ToString(),
                    species = results[0]["species"]!.ToString(),
                    type = results[0]["type"]!.ToString(),
                    gender = results[0]["gender"]!.ToString(),
                    origin = origin,
                    episode = results[0]["episode"]!.Values<string>().ToList()!,
                 };
            return toReturn;
            }
            return null;
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
            if (dynamicObject["error"]==null)
            {
               var episodeCode = dynamicObject["results"][0]["id"].ToString();
               return Convert.ToString(episodeCode); 
            }
            return "Episode Not Found";
        }
    }

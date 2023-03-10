using Newtonsoft.Json;

namespace upswotProj.Models
{
    public enum CheckPersonResult
    {
        False=0,
        True=1,
        NotFound = 2
    }
    public class Person
    {
        public string name { get; set; }
        public string status { get; set; }
        public string species { get; set; }
        public string type { get; set; }
        public string gender { get; set; }
        public Origin origin { get; set; }
        [JsonIgnore]
        public IEnumerable<string> episode { get; set; }
    }
}

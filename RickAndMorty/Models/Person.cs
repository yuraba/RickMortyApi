namespace upswotProj.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string species { get; set; }
        public string type { get; set; }
        public string gender { get; set; }
        public Origin origin { get; set; }
        public IEnumerable<string> episode { get; set; }
    }
}

using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    public class Availability
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
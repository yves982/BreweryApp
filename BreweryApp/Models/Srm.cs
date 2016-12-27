using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    public class Srm
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "hex")]
        public string Hex { get; set; }
    }
}

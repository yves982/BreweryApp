using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    public class BeerHolder
    {
        [DataMember( Name = "data" )]
        public Beer Beer { get; set; }
    }
}

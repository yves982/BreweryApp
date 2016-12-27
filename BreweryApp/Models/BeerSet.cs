using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    [KnownType(typeof(Beer))]
    public class BeerSet
    {
        [DataMember(Name = "currentPage")]
        public int CurrentPage { get; set; }

        [DataMember(Name = "numberOfPages")]
        public int NumberOfPages { get; set; }

        [DataMember(Name = "totalResults")]
        public int TotalResults { get; set; }
        
        [DataMember(Name = "data")]
        public List<Beer> Beers { get; set; }

        public BeerSet()
        {
            Beers = new List<Beer>();
        }
    }
}

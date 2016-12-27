using System;
using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    [KnownType(typeof(Uri))]
    public class Labels
    {
        [DataMember( Name = "icon" )]
        public Uri Icon { get; set; }

        [DataMember(Name = "medium")]
        public Uri Medium { get; set; }

        [DataMember(Name = "large")]
        public Uri Large { get; set; }
    }
}
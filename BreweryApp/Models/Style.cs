using System;
using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    [KnownType(typeof(DateTime?))]
    public class Style
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember(Name = "category")]
        public Category Category { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "ibuMin")]
        public float IbuMin { get; set; }

        [DataMember(Name = "ibuMax")]
        public float IbuMax { get; set; }

        [DataMember(Name = "abvMax")]
        public float AbvMax { get; set; }

        [DataMember(Name = "srmMin")]
        public int SrmMin { get; set; }

        [DataMember(Name = "srmMax")]
        public int SrmMax { get; set; }

        [DataMember(Name = "ogMin")]
        public float OgMin { get; set; }

        [DataMember(Name = "fgMin")]
        public float FgMin { get; set; }

        [DataMember(Name = "fgMax")]
        public float FgMax { get; set; }

        [DataMember(Name = "createDate")]
        public DateTime? CreateDate { get; set; }

        [DataMember(Name = "updateDate")]
        public DateTime? UpdateDate { get; set; }
    }
}
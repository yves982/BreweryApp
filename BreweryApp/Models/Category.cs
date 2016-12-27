using System;
using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    [KnownType(typeof(DateTime?))]
    public class Category
    {
        [DataMember( Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember( Name = "createDate" )]
        public DateTime? CreateDate { get; set; }

        [DataMember(Name = "updateDate")]
        public DateTime? UpdateDate { get; set; }
    }
}
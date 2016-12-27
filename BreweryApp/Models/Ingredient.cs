﻿using System;
using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    [KnownType(typeof(DateTime?))]
    public class Ingredient
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "categoryDisplay")]
        public string CategoryDisplay { get; set; }

        [DataMember(Name = "createDate")]
        public DateTime? CreateDate { get; set; }

        [DataMember(Name = "updateDate")]
        public DateTime? UpdateDate { get; set; }
    }
}

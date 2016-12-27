using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    [KnownType(typeof(Glass))]
    [KnownType(typeof(Style))]
    [KnownType(typeof(Labels))]
    [KnownType(typeof(Availability))]
    [KnownType(typeof(DateTime?))]
    public class Beer
    {
        private string _isOrganicString = "N";
        private bool _isOrganic;

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "nameDisplay")]
        public string DisplayName { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "foodPairings")]
        public string FoodPairing { get; set; }

        [DataMember(Name = "originalGravity")]
        public string OriginalGravity { get; set; }

        [DataMember(Name = "abv")]
        public string Abv { get; set; }

        [DataMember(Name = "ibu")]
        public float Ibu { get; set; }

        [DataMember(Name = "glass")]
        public Glass Glass { get; set; }

        [DataMember(Name = "style")]
        public Style Style { get; set; }

        [DataMember(Name = "isOrganic")]
        public string IsOrganicString
        {
            get { return _isOrganicString; }
            set
            {
                _isOrganicString = value;
                _isOrganic = value == "Y";
            }
        }

        public bool IsOrganic
        {
            get { return _isOrganic; }
            set
            {
                _isOrganic = value;
                _isOrganicString = _isOrganic ? "Y" : "N";
            }
        }

        [DataMember(Name = "labels")]
        public Labels Labels { get; set; }

        [DataMember(Name = "servingTemperature")]
        public string ServingTemperature { get; set; }

        [DataMember(Name = "servingTemperatureDisplay")]
        public string ServingTemperatureDisplay { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }
        
        [DataMember(Name = "statusDisplay")]
        public string StatusDisplay { get; set; }

        [DataMember(Name = "srm")]
        public Srm Srm { get; set; }

        [DataMember(Name = "available")]
        public Availability Available { get; set; }

        [DataMember(Name = "beerVariationId")]
        public string BeerVariationId { get; set; }

        [DataMember(Name = "year")]
        public int? Year { get; set; }

        [DataMember(Name = "createDate")]
        public DateTime? CreateDate { get; set; }

        [DataMember(Name = "updateDate")]
        public DateTime? UpdateDate { get; set; }

        [IgnoreDataMember]
        public List<Ingredient> Ingredients { get; set; }

        public Beer()
        {
            Ingredients = new List<Ingredient>();
        }
    }
}
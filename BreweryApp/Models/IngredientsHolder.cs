using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    [KnownType(typeof(Ingredient))]
    public class IngredientsHolder
    {
        [DataMember( Name = "data" )]
        public List<Ingredient> Ingredients { get; set; }
    }
}

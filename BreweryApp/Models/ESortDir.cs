using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    public enum ESortDir
    {
        [EnumMember( Value = "ASC" )]
        Asc,

        [EnumMember(Value = "DESC")]
        Desc
    }
}

using System.Runtime.Serialization;

namespace BreweryApp.Models
{
    [DataContract]
    public enum EOrderField
    {
        [EnumMember( Value = "name" )]
        Name,

        [EnumMember(Value = "description")]
        Description,

        [EnumMember(Value = "abv")]
        Abv,

        [EnumMember(Value = "ibu")]
        Ibu,

        [EnumMember(Value = "glasswareId")]
        GlasswareId,

        [EnumMember(Value = "srmId")]
        SrmId,

        [EnumMember(Value = "availableId")]
        AvailableId,

        [EnumMember(Value = "styleId")]
        StyleId,

        [EnumMember(Value = "isOrganic")]
        IsOrganic,

        [EnumMember(Value = "status")]
        Status,

        [EnumMember(Value = "createDate")]
        CreateDate,

        [EnumMember(Value = "updateDate")]
        UpdateDate,

        [EnumMember(Value = "random")]
        Random
    }
}

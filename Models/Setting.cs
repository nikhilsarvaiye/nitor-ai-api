namespace Models
{
    using MongoDB.Bson.Serialization.Attributes;

    public class Setting : BaseModel
    {
        public SettingType Type { get; set; }
    }
}

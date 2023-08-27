using System.Text.Json.Serialization;

namespace EntityFrameworkMetaTest.Model.Meta
{
    public class MetaTable
    {
        [JsonPropertyName("tablename")]
        public string TableName { get; set; }

        [JsonPropertyName("storage")]
        public string Storage { get; set; }

        [JsonPropertyName("columns")]
        public MetaColumns[] Columns { get; set; }
    }


}

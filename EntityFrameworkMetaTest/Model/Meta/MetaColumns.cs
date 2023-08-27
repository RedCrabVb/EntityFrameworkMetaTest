using System.Text.Json.Serialization;

namespace EntityFrameworkMetaTest.Model.Meta
{
    public class MetaColumns
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("key")]
        public bool Key { get; set; }
    }
}

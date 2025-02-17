using System.Text.Json.Serialization;

namespace ValorantStatusWebView.DataTransferObjects
{
    public class UpdateDto
    {
        public int Id { get; set; }

        public required string Author { get; set; }

        public bool Publish { get; set; }

        [JsonPropertyName("publish_locations")]
        public required List<string> PublishLocations { get; set; }

        public required List<ContentDto> Translations { get; set; }

        [JsonPropertyName("created_at")]
        public required string CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public required string UpdatedAt { get; set; }
    }
}

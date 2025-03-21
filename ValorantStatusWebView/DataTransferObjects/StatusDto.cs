using System.Text.Json.Serialization;

namespace ValorantStatusWebView.DataTransferObjects
{
    public class StatusDto
    {
        public int Id { get; set; }

        [JsonPropertyName("maintenance_status")]
        public required string MaintenanceStatus { get; set; }

        [JsonPropertyName("incident_severity")]
        public required string IncidentSeverity { get; set; }

        public required List<ContentDto> Titles { get; set; }

        public required List<UpdateDto> Updates { get; set; }

        [JsonPropertyName("created_at")]
        public required string CreatedAt { get; set; }

        [JsonPropertyName("archive_at")]
        public required string ArchivedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public required string UpdatedAt { get; set; }

        public required List<string> Platforms { get; set; }
    }
}

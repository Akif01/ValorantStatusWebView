namespace ValorantStatusWebView.DataTransferObjects
{
    public class StatusDto
    {
        public int Id { get; set; }
        public string MaintenanceStatus { get; set; }
        public string IncidentSeverity { get; set; }
        public List<ContentDto> Titles { get; set; }
        public List<UpdateDto> Updates { get; set; }
        public string CreatedAt { get; set; }
        public string ArchivedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Platforms { get; set; }
    }
}

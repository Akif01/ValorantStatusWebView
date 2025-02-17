namespace ValorantStatusWebView.DataTransferObjects
{
    public class PlatformDataDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required List<string> Locales { get; set; }
        public required List<StatusDto> Maintenances { get; set; }
        public required List<StatusDto> Incidents { get; set; }
    }
}

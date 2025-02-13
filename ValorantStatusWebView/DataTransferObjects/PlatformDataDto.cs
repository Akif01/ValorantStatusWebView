namespace ValorantStatusWebView.DataTransferObjects
{
    public class PlatformDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Locales { get; set; }
        public List<StatusDto> Maintenances { get; set; }
        public List<StatusDto> Incidents { get; set; }
    }
}

namespace ValorantStatusWebView.DataTransferObjects
{
    public class UpdateDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public bool Publish { get; set; }
        public List<string> PublishLocations { get; set; }
        public List<ContentDto> Translations { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}

using System.Text.Json;

namespace ValorantStatusWebView.DataTransferObjects
{
    public class ContentDto
    {
        public required string Locale { get; set; }
        public required string Content { get; set; }
    }
}

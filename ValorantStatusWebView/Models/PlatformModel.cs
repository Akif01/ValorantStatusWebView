using ValorantStatusWebView.DataTransferObjects;

namespace ValorantStatusWebView.Models
{
    public class PlatformModel : ModelBase<PlatformDataDto>
    {
        public string RegionName { get; private set; } = string.Empty;
        public bool IsAvailable { get; private set; }

        public PlatformModel(PlatformDataDto dto) : base(dto) { }

        protected override void Build(PlatformDataDto dto)
        {
            RegionName = dto.Name;
            IsAvailable = dto.Maintenances.Count == 0 && dto.Incidents.Count == 0;
        }
    }
}

using ValorantStatusWebView.DataTransferObjects;

namespace ValorantStatusWebView.Models
{
    public class MaintenanceModel : ModelBase<StatusDto>
    {
        public string MaintenanceStatus { get; private set; } = string.Empty;

        public MaintenanceModel(StatusDto dto) : base(dto)
        {
        }

        protected override void Build(StatusDto dto)
        {
            MaintenanceStatus = dto.MaintenanceStatus;
        }
    }
}

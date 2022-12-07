using MakoIoT.ConfigurationApi.Model;
using MakoIoT.Core.Configuration.App.Client.Model;
using MakoIoT.Core.Configuration.App.Client.Services;

namespace MakoIoT.Core.Configuration.App.Client.ViewModels
{
    public class SectionViewModel
    {
        private readonly IDeviceConfigService _deviceConfigService;
        private readonly IMessage _message;

        public string Url { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public bool IsHidden { get; set; }
        public bool IsCollapsed { get; set; } = true;
        public string CollapsedString => IsCollapsed ? "collapsed" : String.Empty;
        public string ShowString => IsCollapsed ? String.Empty : "show";
        public List<ConfigParamViewModel> ConfigParams { get; set; }

        public SectionViewModel(IDeviceConfigService deviceConfigService, IMessage message)
        {
            _deviceConfigService = deviceConfigService;
            _message = message;
        }
        public async Task ToggleSection()
        {
            IsCollapsed = !IsCollapsed;
            if (!IsCollapsed)
            {
                try
                {
                    var section = await _deviceConfigService.GetSection(Url, Name);
                    var metadata = await _deviceConfigService.GetSectionMetadata(Url, Name);

                    ConfigParams = (
                        from s in section.Parameters
                        from m in (metadata == null ? Array.Empty<ConfigParamMetadata>()
                            : metadata.Parameters).Where(x => x.Name == s.Name).DefaultIfEmpty()
                        select new ConfigParamViewModel
                        {
                            Name = s.Name,
                            Label = m?.Label ?? s.Name,
                            Value = s.Value?.ToString() ?? String.Empty,
                            IsHidden = m?.IsHidden ?? false,
                            IsSecret = m?.IsSecret ?? false,
                            DefaultValue = m?.DefaultValue,
                            Type = m?.Type ?? s.Type
                        }).ToList();


                    if (metadata != null)
                    {
                        Label = metadata.Label;
                        IsHidden = metadata.IsHidden;
                    }
                }
                catch
                {
                    _message.DisplayMessage($"Getting \"{Name}\" section's details failed", MessageType.Error);
                }
            }
        }

        public async Task UpdateSection()
        {
            _message.DisplayMessage($"Updating \"{Name}\" section's details...", MessageType.Info);
            var section = new Section(Name, 
                ConfigParams.Select(p => new Parameter(p.Name, p.Value, p.Type)));

            try
            {
                await _deviceConfigService.PostSection(Url, Name, section);
                _message.DisplayMessage($"Section \"{Name}\" updated successfully", MessageType.Success);
            }
            catch
            {
                _message.DisplayMessage($"Updating \"{Name}\" section failed", MessageType.Error);
            }
        }
    }
}

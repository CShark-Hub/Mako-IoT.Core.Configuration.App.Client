using MakoIoT.ConfigurationApi.Model;
using MakoIoT.Core.Configuration.App.Client.Services;

namespace MakoIoT.Core.Configuration.App.Client.ViewModels
{
    public class DeviceViewModel
    {
        private readonly IDeviceConfigService _deviceConfigService;
        private readonly IMessage _message;
        public IEnumerable<SectionViewModel> Sections { get; set; }

        public string Url { get; set; } = "192.168.4.1";
        public string Key { get; set; }
        public string DeviceName { get; set; }
        public string DeviceManufacturer { get; set; }
        public string DeviceSerialNo { get; set; }


        public DeviceViewModel(IDeviceConfigService deviceConfigService, IMessage message)
        {
            _deviceConfigService = deviceConfigService;
            _message = message;
            Sections = Array.Empty<SectionViewModel>();
        }

        public async Task Connect()
        {
            Sections = Array.Empty<SectionViewModel>();
            _message.DisplayMessage($"Connecting to device at {Url} ...", MessageType.Info);
            try
            {
                var deviceData = await _deviceConfigService.GetDevice(Url);
                if (deviceData != null)
                {
                    DeviceName = deviceData.Name;
                    DeviceManufacturer = deviceData.Manufacturer;
                    DeviceSerialNo = deviceData.SerialNo;
                }

                var sections = await _deviceConfigService.GetSections(Url);
                Sections = 
                    (from s in sections
                    from d in (deviceData == null ? Array.Empty<ConfigSectionMetadata>()
                            : deviceData.ConfigSections).Where(x=>x.Name.ToLower() == s.ToLower()).DefaultIfEmpty()
                    select new SectionViewModel(_deviceConfigService, _message)
                    {
                        Url = Url,
                        Name = s,
                        Label = d?.Label ?? s,
                        IsHidden = d?.IsHidden ?? deviceData?.HideOtherSections ?? false
                    }).ToList();
                _message.HideMessage();
            }
            catch
            {
                _message.DisplayMessage($"Cannot connect to device at {Url}", MessageType.Error);
            }

        }
        public async Task ExitConfigMode()
        {
            _message.DisplayMessage($"Connecting to device at {Url} ...", MessageType.Info);
            try
            {
                await _deviceConfigService.Exit(Url);
                _message.DisplayMessage("Device exited configuration mode", MessageType.Success);
            }
            catch
            {
                _message.DisplayMessage($"Cannot connect to device at {Url}", MessageType.Error);
            }
        }
    }
}

using MakoIoT.ConfigurationApi.Model;
using MakoIoT.Core.Configuration.App.Client.Model;

namespace MakoIoT.Core.Configuration.App.Client.Services;

public interface IDeviceConfigService
{
    Task<IEnumerable<string>> GetSections(string baseUrl);
    Task<Section> GetSection(string? baseUrl, string? name);
    Task PostSection(string? baseUrl, string? name, Section section);
    Task Exit(string baseUrl);
    Task<ConfigSectionMetadata?> GetSectionMetadata(string? baseUrl, string? name);
    Task<DeviceMetadata?> GetDevice(string baseUrl);
}
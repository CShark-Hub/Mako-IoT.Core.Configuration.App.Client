using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MakoIoT.ConfigurationApi.Model;
using MakoIoT.Core.Configuration.App.Client.Model;

namespace MakoIoT.Core.Configuration.App.Client.Services
{
    public class DeviceConfigService : IDeviceConfigService
    {
        public const string ClientName = "Device";

        private readonly IHttpClientFactory _clientFactory;

        public DeviceConfigService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<string>> GetSections(string baseUrl)
        {
            using var client = _clientFactory.CreateClient(ClientName);
            var response = await client.GetFromJsonAsync<string[]>($"http://{baseUrl}/config/sections");
            return response ?? Array.Empty<string>();
        }

        public async Task<Section> GetSection(string baseUrl, string name)
        {
            using var client = _clientFactory.CreateClient(ClientName);
            var response = await client.GetFromJsonAsync<Dictionary<string, dynamic?>>($"http://{baseUrl}/config/section/{name}");
            var section = new Section(name,
                response?.Select(i => new Parameter(i.Key, i.Value?.ToString()))
                ?? Array.Empty<Parameter>()
            );
            return section;
        }

        public async Task<ConfigSectionMetadata?> GetSectionMetadata(string baseUrl, string name)
        {
            using var client = _clientFactory.CreateClient(ClientName);
            var response = await client.GetAsync($"http://{baseUrl}/config/metadata/{name}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            response.EnsureSuccessStatusCode();
            var o = await JsonSerializer.DeserializeAsync<ConfigSectionMetadata>(await response.Content.ReadAsStreamAsync());
            return o;
        }

        public async Task<DeviceMetadata?> GetDevice(string baseUrl)
        {
            using var client = _clientFactory.CreateClient(ClientName);
            var response = await client.GetAsync($"http://{baseUrl}/device/metadata");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            response.EnsureSuccessStatusCode();
            var o = await JsonSerializer.DeserializeAsync<DeviceMetadata>(await response.Content.ReadAsStreamAsync());
            return o;
        }

        public async Task PostSection(string baseUrl, string name, Section section)
        {
            var obj = section.Parameters.ToDictionary(
                k => k.Name, v => ConvertValue(v.Value, v.Type));

            using var client = _clientFactory.CreateClient(ClientName);
            var response = await client.PostAsJsonAsync($"http://{baseUrl}/config/section/{name}", obj);
            response.EnsureSuccessStatusCode();
        }


        public async Task Exit(string baseUrl)
        {
            using var client = _clientFactory.CreateClient(ClientName);
            var response = await client.GetAsync($"http://{baseUrl}/config/exit");
            response.EnsureSuccessStatusCode();
        }

        private static object? ConvertValue(string? value, string? type)
        {
            if (value == null)
                return null;

            if (type == null)
                return value;

            switch (type.ToLower())
            {
                //CLR types
                case "string": return value;
                case "int": return Convert.ToInt32(value);
                case "bool": return Convert.ToBoolean(value);
                case "float": return Convert.ToSingle(value);
                case "double": return Convert.ToDouble(value);
                case "datetime": return Convert.ToDateTime(value);
                //special types
                case "timezone": return value;
                case "text": return value;

                default: throw new NotSupportedException($"Type \"{type}\" not supported");
            }
        }
    }
}

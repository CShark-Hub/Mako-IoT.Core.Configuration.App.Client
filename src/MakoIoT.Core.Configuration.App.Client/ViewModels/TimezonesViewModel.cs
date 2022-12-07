using Blazored.Modal.Services;
using TimeZoneConverter.Posix;

namespace MakoIoT.Core.Configuration.App.Client.ViewModels
{
    public class TimezonesViewModel
    {
        private readonly IEnumerable<TimeZoneInfo> _timezones = TimeZoneInfo.GetSystemTimeZones();

        public string? SelectedTimezone { get; set; }
        public IEnumerable<TimeZoneInfo> AllTimezones => _timezones;

        public string? TimezoneString { get; set; }

        public async Task Ok()
        {
            TimezoneString = null;
            if (SelectedTimezone != null)
            {
                TimezoneString = PosixTimeZone.FromTimeZoneInfo(
                    TimeZoneInfo.FindSystemTimeZoneById(SelectedTimezone));
            }
        }

        public async Task Cancel()
        {
            TimezoneString = null;
        }
    }
}

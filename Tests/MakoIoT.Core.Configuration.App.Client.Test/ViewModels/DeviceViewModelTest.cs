using MakoIoT.ConfigurationApi.Model;
using MakoIoT.Core.Configuration.App.Client.Services;
using MakoIoT.Core.Configuration.App.Client.ViewModels;
using Moq;
using Xunit.Abstractions;

namespace MakoIoT.Core.Configuration.App.Client.Test.ViewModels
{
    public class DeviceViewModelTest
    {
        private readonly ITestOutputHelper _output;

        public DeviceViewModelTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Connect_given_no_device_data_should_display_all_sections()
        {
            var sections = new string[] { "section1", "section2" };

            var dcs = new Mock<IDeviceConfigService>();
            dcs.Setup(s => s.GetDevice(It.IsAny<string>()))
                .ReturnsAsync((DeviceMetadata?)null);
            dcs.Setup(s => s.GetSections(It.IsAny<string>()))
                .ReturnsAsync(sections);

            var sut = new DeviceViewModel(dcs.Object, new Mock<IMessage>().Object);

            await sut.Connect();

            Assert.Equal(sections.Length, sut.Sections.Count());
            Assert.All(sut.Sections, s => Assert.False(s.IsHidden));
        }

        [Fact]
        public async Task Connect_given_device_data_should_display_all_sections()
        {
            var sections = new[] { "section1", "section2", "section3" };

            var deviceData = new DeviceMetadata
            {
                ConfigSections = new[]
                {
                    new ConfigSectionMetadata { Name = sections[0], Label = "section1 label" },
                    new ConfigSectionMetadata { Name = sections[1], Label = "section2 label" },
                }
            };
            

            var dcs = new Mock<IDeviceConfigService>();
            dcs.Setup(s => s.GetDevice(It.IsAny<string>()))
                .ReturnsAsync(deviceData);
            dcs.Setup(s => s.GetSections(It.IsAny<string>()))
                .ReturnsAsync(sections);

            var sut = new DeviceViewModel(dcs.Object, new Mock<IMessage>().Object);

            await sut.Connect();

            var sectionsArray = sut.Sections.ToArray();
            Assert.Equal(sections.Length, sectionsArray.Length);
            Assert.All(sectionsArray, s => Assert.False(s.IsHidden));
            Assert.Equal(deviceData.ConfigSections[0].Label, sectionsArray[0].Label);
            Assert.Equal(deviceData.ConfigSections[1].Label, sectionsArray[1].Label);
            Assert.Equal(sections[2], sectionsArray[2].Label);
        }

        [Fact]
        public async Task Connect_given_HideOtherSections_should_set_IsHidden_on_other_sections()
        {
            var sections = new[] { "section1", "section2", "section3" };

            var deviceData = new DeviceMetadata
            {
                ConfigSections = new[]
                {
                    new ConfigSectionMetadata { Name = sections[0], Label = "section1 label" },
                    new ConfigSectionMetadata { Name = sections[1], Label = "section2 label" },
                },
                HideOtherSections = true
            };


            var dcs = new Mock<IDeviceConfigService>();
            dcs.Setup(s => s.GetDevice(It.IsAny<string>()))
                .ReturnsAsync(deviceData);
            dcs.Setup(s => s.GetSections(It.IsAny<string>()))
                .ReturnsAsync(sections);

            var sut = new DeviceViewModel(dcs.Object, new Mock<IMessage>().Object);

            await sut.Connect();

            var sectionsArray = sut.Sections.ToArray();
            Assert.Equal(sections.Length, sectionsArray.Length);
            Assert.False(sectionsArray[0].IsHidden);
            Assert.False(sectionsArray[1].IsHidden);
            Assert.True(sectionsArray[2].IsHidden);
        }
    }
}

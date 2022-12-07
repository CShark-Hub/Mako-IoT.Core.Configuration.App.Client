using MakoIoT.ConfigurationApi.Model;
using MakoIoT.Core.Configuration.App.Client.Model;
using MakoIoT.Core.Configuration.App.Client.Services;
using MakoIoT.Core.Configuration.App.Client.ViewModels;
using Moq;
using Xunit.Abstractions;

namespace MakoIoT.Core.Configuration.App.Client.Test.ViewModels
{
    public class SectionViewModelTest
    {
        private readonly ITestOutputHelper _output;

        public SectionViewModelTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ToggleSection_given_null_metadata_should_set_section_details()
        {
            var section = new Section("test", new Parameter[] { new Parameter("param1", "value1") });

            var dcs = new Mock<IDeviceConfigService>();
            dcs.Setup(s => s.GetSection(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(section);

            dcs.Setup(s => s.GetSectionMetadata(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((ConfigSectionMetadata?)null);

            var sut = new SectionViewModel(dcs.Object, new Mock<IMessage>().Object);

            await sut.ToggleSection();

            Assert.Single(sut.ConfigParams);
            Assert.Equal(section.Parameters.First().Name, sut.ConfigParams[0].Name);
            Assert.Equal(section.Parameters.First().Value, sut.ConfigParams[0].Value);
        }

        [Fact]
        public async Task ToggleSection_should_set_section_metadata()
        {
            var section = new Section("test", new Parameter[] { new Parameter("param1", "value1") });

            var metadata = new ConfigSectionMetadata
            {
                Name = "test",
                IsHidden = true,
                Label = "metadata test",
                Parameters = new ConfigParamMetadata[]
                {
                    new ConfigParamMetadata
                    {
                        Name = "param1", IsHidden = true, Label = "metadata param1", DefaultValue = "default",
                        IsSecret = true, Type = "type"
                    }
                }
            };

            var dcs = new Mock<IDeviceConfigService>();
            dcs.Setup(s => s.GetSection(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(section);

            dcs.Setup(s => s.GetSectionMetadata(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(metadata);

            var sut = new SectionViewModel(dcs.Object, new Mock<IMessage>().Object);

            await sut.ToggleSection();

            Assert.Single(sut.ConfigParams);
            Assert.Equal(section.Parameters.First().Name, sut.ConfigParams[0].Name);
            Assert.Equal(section.Parameters.First().Value, sut.ConfigParams[0].Value);
            Assert.Equal(metadata.Label, sut.Label);
            Assert.Equal(metadata.IsHidden, sut.IsHidden);
            Assert.Equal(metadata.Parameters.First().Label, sut.ConfigParams[0].Label);
            Assert.Equal(metadata.Parameters.First().IsHidden, sut.ConfigParams[0].IsHidden);
            Assert.Equal(metadata.Parameters.First().IsSecret, sut.ConfigParams[0].IsSecret);
            Assert.Equal(metadata.Parameters.First().DefaultValue, sut.ConfigParams[0].DefaultValue);
            Assert.Equal(metadata.Parameters.First().Type, sut.ConfigParams[0].Type);

        }

    }
}

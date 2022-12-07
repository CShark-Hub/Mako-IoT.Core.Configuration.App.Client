using MakoIoT.ConfigurationApi.Model;

namespace MakoIoT.Core.Configuration.App.Client.ViewModels
{
    public class ConfigParamViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public bool IsHidden { get; set; }
        public bool IsSecret { get; set; }
        public object? DefaultValue { get; set; }
        public string Value { get; set; }

        public int ValueInt
        {
            get => int.Parse(Value);
            set => Value = value.ToString();
        }

        public bool ValueBool
        {
            get => bool.Parse(Value);
            set => Value = value.ToString();
        }
        public float ValueFloat
        {
            get => float.Parse(Value);
            set => Value = value.ToString();
        }
        public double ValueDouble
        {
            get => double.Parse(Value);
            set => Value = value.ToString();
        }
        public DateTime ValueDateTime
        {
            get => DateTime.Parse(Value);
            set => Value = value.ToString();
        }
    }
}

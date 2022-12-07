namespace MakoIoT.Core.Configuration.App.Client.Model
{
    public class Parameter
    {
        public Parameter(string name, string? value, string? type = null)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public string Name { get; }
        public string? Value { get; }
        public string? Type { get; }
    }
}

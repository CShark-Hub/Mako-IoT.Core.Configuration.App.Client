namespace MakoIoT.Core.Configuration.App.Client.Model
{
    public class Section
    {
        public Section(string name, IEnumerable<Parameter> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; }
        public IEnumerable<Parameter> Parameters { get; }
    }
}

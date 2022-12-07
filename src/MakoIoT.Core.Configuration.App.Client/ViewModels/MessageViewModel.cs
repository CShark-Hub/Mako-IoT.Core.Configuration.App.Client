namespace MakoIoT.Core.Configuration.App.Client.ViewModels
{
    public class MessageViewModel : IMessage
    {
        public string? MessageText { get; private set; }
        public MessageType MessageType { get; private set; }
        public event EventHandler? ViewUpdated;

        public void DisplayMessage(string text, MessageType type)
        {
            MessageText = text;
            MessageType = type;
            ViewUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void HideMessage()
        {
            MessageText = null;
            ViewUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}

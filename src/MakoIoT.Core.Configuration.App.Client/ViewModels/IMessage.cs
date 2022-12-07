namespace MakoIoT.Core.Configuration.App.Client.ViewModels
{
    public interface IMessage
    {
        void DisplayMessage(string text, MessageType type);
        void HideMessage();
    }
}

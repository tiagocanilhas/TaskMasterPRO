namespace TaskMasterPRO.Data.Services.Interfaces
{
    public interface IDialogServices
    {
        bool AskConfirmation(string message, string title);
        void ShowError(string message, string title);
    }
}

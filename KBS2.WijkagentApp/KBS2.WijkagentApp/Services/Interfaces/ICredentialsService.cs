namespace KBS2.WijkagentApp.Services.Interfaces
{
    public interface ICredentialsService
    {
        string Id { get; }

        string UserName { get; }

        string Password { get; }

        void SaveCredentials(string iD, string userName, string password);

        void DeleteCredentials();

        bool DoCredentialsExist();
    }
}

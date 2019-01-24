using System;

namespace KBS2.WijkagentApp.Services.Interfaces
{
    public interface ICredentialsService
    {
        Guid Guid { get; }

        string Id { get; }

        string UserName { get; }

        string Password { get; }

        void SaveCredentials(Guid iD, string userName, string password);

        void DeleteCredentials();

        bool DoCredentialsExist();
    }
}

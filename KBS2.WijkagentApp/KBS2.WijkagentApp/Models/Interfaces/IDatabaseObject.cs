using System;

namespace KBS2.WijkagentApp.Models.Interfaces
{
    public interface IDatabaseObject
    {
        //member is needed for mobileservices (api)
        Guid Id { get; set; }
    }
}
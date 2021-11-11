using Microsoft.AspNetCore.SignalR;

namespace dotnetcore.Intefaces
{
    public interface IMongoConnection
    {
        string Authenticate(string username, string password);

    }
}

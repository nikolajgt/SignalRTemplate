using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace dotnetcore.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServerHub : DynamicHub
    {

        //Hvad der sker når man connecter og disconnecter
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            Console.WriteLine(Context.ConnectionId);            
            await base.OnDisconnectedAsync(ex);
        }



        //Chat alle, Caller and Group
        public void SendMessage(string user, string message)
        {
           
            Clients.All.SendAsync("broadcastMessage", message);
        }

        public Task SendMessageToCaller(string user, string message)
        {
            return Clients.Caller.SendAsync("broadcastMessage", user, message);
        }

        public Task SendMessageToGroup(string user, string message, string group)
        {
            return Clients.Group(group).SendAsync("broadcastMessage", user, message);
        }




        //Adder til og fra en gruppe
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("GroupSend", $"{Context.ConnectionId} Has joined the group {groupName}.");
        }
            //Kan kun se reponse log i console hvis man er medlem af den gruppe der bliver leaver fra
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("GroupSend", $"{Context.ConnectionId} Has left the group {groupName}.");
        }



        
        
    }
}

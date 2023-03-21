using Microsoft.AspNetCore.SignalR;

namespace MPGame.Api.Hubs;

public class GameHub : Hub
{
    private readonly ILobbiesService lobbiesService;

    public GameHub(ILobbiesService lobbiesService) {
        this.lobbiesService = lobbiesService;
    }

    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", GetUserName(), message);
    }

    public async Task JoinLobby(string lobbyId)
    {
        Console.WriteLine($"{nameof(GameHub)}.{nameof(JoinLobby)}(lobbyId: {lobbyId}) : ConnectionId: {Context.ConnectionId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        // Console.WriteLine($"{nameof(GameHub)}.{nameof(JoinLobby)}(...) added to group");
        lobbiesService.AddToLobby(GetUserName(), lobbyId);
        // Console.WriteLine($"{nameof(GameHub)}.{nameof(JoinLobby)}(...) added to lobby");

        // await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    public async Task LeaveLobby(string lobbyId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);

        lobbiesService.RemoveFromLobby(GetUserName(), lobbyId);

        // await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
    }

    private string GetUserName()
    {
        return $"Anonymous {Context.ConnectionId.Substring(0, 5)}";
    }

    #region Dependencies
    public interface ILobbiesService
    {
        void AddToLobby(string userName, string lobbyId);
        void RemoveFromLobby(string userName, string lobbyId);
    }
    #endregion Dependencies
}
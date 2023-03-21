using MPGame.Api.Hubs;
using MPGame.Api.Models;

namespace MPGame.Api.Services;

public class LobbiesService : LobbiesController.ILobbiesService, GameHub.ILobbiesService
{
    #region LobbiesController.ILobbiesService
    IEnumerable<LobbyModel> LobbiesController.ILobbiesService.GetLobbies() => lobbies.Values;
    LobbyModel LobbiesController.ILobbiesService.GetLobby(string lobbyId) => lobbies[lobbyId];
    string LobbiesController.ILobbiesService.GenerateNewLobbyName() => GenerateNewLobbyName();
    #endregion LobbiesController.ILobbiesService

    #region GameHub.ILobbiesService
    void GameHub.ILobbiesService.AddToLobby(string userName, string lobbyId) => AddToLobby(userName, lobbyId);
    void GameHub.ILobbiesService.RemoveFromLobby(string userName, string lobbyId) => RemoveFromLobby(userName, lobbyId);
    #endregion GameHub.ILobbiesService

    #region Implementation
    private const string lobbyIdSource = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private static Random random = new();
    private readonly Dictionary<string, LobbyModel> lobbies = new();

    private void AddToLobby(string userName, string lobbyId)
    {
        if(!lobbies.ContainsKey(lobbyId)) {
            lobbies[lobbyId] = new(lobbyId);
        }

        lobbies[lobbyId].Users.Add(userName);
    }

    private void RemoveFromLobby(string userName, string lobbyId)
    {
        if(!lobbies.ContainsKey(lobbyId)) {
            return;
        }

        lobbies[lobbyId].Users.Remove(userName);

        if(!lobbies[lobbyId].Users.Any()) {
            lobbies.Remove(lobbyId);
        }
    }

    private string GenerateNewLobbyName()
    {
        string lobbyId;
        
        do {
            lobbyId = new String(
                Enumerable
                    .Repeat(0, 8)
                    .Select(i => 
                        lobbyIdSource[random.Next(lobbyIdSource.Length)]
                    ).ToArray()
            );
        } while(lobbies.ContainsKey(lobbyId));

        return lobbyId;
    }
    #endregion Implementation
}
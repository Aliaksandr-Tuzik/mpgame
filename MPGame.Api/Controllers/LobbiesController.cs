using Microsoft.AspNetCore.Mvc;
using MPGame.Api.Models;
using MPGame.Shared.Models;

[ApiController]
[Route("[controller]")]
public class LobbiesController : ControllerBase {
    private readonly ILobbiesService lobbiesService;

    public LobbiesController(ILobbiesService lobbiesService)
    {
        this.lobbiesService = lobbiesService;
    }

    [HttpGet(Name = "GetAvailableLobbies")]
    public IEnumerable<LobbyInfo> Get()
    {
        return lobbiesService.GetLobbies()
            .Select(lobby => new LobbyInfo(lobby.Id))
            .ToArray();
    }

    [HttpGet("/{lobbyId}", Name = "GetLobbyById")]
    public LobbyDetails Get(string lobbyId)
    {
        var lobbyModel = lobbiesService.GetLobby(lobbyId);

        return new LobbyDetails(lobbyModel.Id);
    }

    [HttpPost(Name = "CreateNewLobby")]
    public LobbyInfo Post()
    {
        Console.WriteLine($"{nameof(LobbiesController)}.{nameof(Post)}()");
        var lobbyName = lobbiesService.GenerateNewLobbyName();
        Console.WriteLine($"{nameof(LobbiesController)}.{nameof(Post)}(): lobbyName = {lobbyName}");
        return new LobbyInfo(lobbyName);
    }

    #region Dependencies
    public interface ILobbiesService
    {
        IEnumerable<LobbyModel> GetLobbies();

        LobbyModel GetLobby(string lobbyId);

        string GenerateNewLobbyName();
    }
    #endregion Dependencies
}
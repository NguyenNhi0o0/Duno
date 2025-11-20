using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    private Dictionary<int, Lobby> lobbies = new();

    public Dictionary<int, NetworkPlayer> playerConnections = new();

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public Lobby CreateLobby(Player hostPlayer) {
        Lobby newLobby = new(hostPlayer);
        lobbies.Add(newLobby.Id, newLobby);
        return newLobby;
    }

    public int JoinLobby(int lobbyId, Player player) {
        if (lobbies.TryGetValue(lobbyId, out Lobby lobby)) {
            return lobby.AddPlayer(player);
        }
        // The lobby does not exist
        return -4;
    }

    public int LeaveLobby(int lobbyId, string playerName) {
        if (lobbies.TryGetValue(lobbyId, out Lobby lobby)) {
            return lobby.RemovePlayer(playerName);
        }
        // The lobby does not exist
        return -2;
    }

    public bool DestroyLobby(int lobbyId) {
        if (lobbies.ContainsKey(lobbyId)) {
            lobbies.Remove(lobbyId);
            return true;
        }
        return false;
    }

    public Lobby GetLobbyById(int lobbyId) {
        if (lobbies.TryGetValue(lobbyId, out Lobby lobby)) {
            return lobby;
        }
        return null;
    }

    public bool IsInLobby(Player player) {
        foreach (var lobby in lobbies.Values) {
            if (lobby.Players.Contains(player)) {
                return true;
            }
        }
        return false;
    }

    public void RegisterPlayer(Player logical, NetworkPlayer net) {
        playerConnections[logical.Id] = net;
    }

    public NetworkPlayer GetNetworkPlayer(Player logical) {
        return playerConnections.TryGetValue(logical.Id, out var net) ? net : null;
    }

    public NetworkConnectionToClient GetConnection(Player logical) {
        return GetNetworkPlayer(logical)?.connectionToClient;
    }


/*PAS UTILE POUR LE MOMENT*/
    public List<Lobby> GetLobbies() {
        List<Lobby> result = new();
        foreach (var lobby in lobbies.Values) {
            if (!lobby.IsPrivate && !lobby.IsGameStarted) {
                result.Add(lobby);
            }
        }
    return result;
    }
}
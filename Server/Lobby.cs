using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Lobby
{
    private static int lobbyIdCounter = 0;
    private const int MAX_PLAYERS = 4;

    public int Id { get; private set; }
    public List<Player> Players = new List<Player>();
    public string LobbyName = "Default lobby";
    public string HostName = "Default host";
    public bool IsPrivate = false;
    public bool IsGameStarted = false;
    public int GameMode = 0;
    public Game GameInstance; 

    public Lobby(Player hostPlayer) {
        Id = ++lobbyIdCounter;
        //LobbyName = $"{hostPlayer.Name}'s game";
        //HostName = hostPlayer.Name;
        //Players.Add(hostPlayer);
    }

    public int AddPlayer(Player player) {
        // The player is already in the lobby
        if (Players.Contains(player)) {
            return -1;
        }
        // The game has already started
        if (IsGameStarted) {
            return -2;
        }
        // The lobby is full
        if (Players.Count >= MAX_PLAYERS) {
            return -3;
        }
        // The player join the lobby
        Players.Add(player);
        // peut etre modifier plus tard
        if (Players.Count == 1) {
            HostName = player.Name;
        }
        return 1;
    }

    public int RemovePlayer(string playerName) {
        Player playerToRemove = Players.Find(p => p.Name == playerName);
        if (playerToRemove != null) {
            Players.Remove(playerToRemove);
            if (playerName == HostName) {
                Players.Clear();
                // destroyGame si le jeu est commencé !!!
                LobbyManager.Instance.DestroyLobby(Id);
            }
            return 1;
        }
        // The player is not in the lobby
        return -1;
    }

    public int StartGame() {
        if (IsGameStarted) {
            return -1;
        }
        if (Players.Count < 1) { // changer en 2 joueur minimum
            return -2;
        }
        GameInstance = new Game(Players, GameMode);
        GameInstance.Start();
        IsGameStarted = true;
        return 1;
    }


// pas utiliser pour le moment
    public void StopGame() {
        if (GameInstance != null) {
            GameInstance = null;
            IsGameStarted = false;
        }
    }
}
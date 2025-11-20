using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public struct CardInfo {
    public CardColor CardC;
    public CardValue CardV;
    public int CardId;
}

public struct PlayerInfo {
    public int Index;
    public int NbCartes;
}

public struct Info
{
    public CardInfo[] Hand;
    public PlayerInfo[] TabPlayers;
    public CardInfo CurrentCard;
    public PlayerInfo CurrentPlayer;
}

public class Commands : NetworkBehaviour
{
    [Command]
    public void CmdCreateLobby() {
        // Get the player attached to the connection
        Player player = connectionToClient.identity.GetComponent<NetworkPlayer>().PlayerData;
        // Check if the player is already in a lobby
        if (LobbyManager.Instance.IsInLobby(player)) {
            Debug.LogWarning($"{player.Name} est déjà dans un lobby");
            return;
        }
        // Create a new lobby
        Lobby newLobby = LobbyManager.Instance.CreateLobby(player);
        Debug.Log($"Lobby {newLobby.Id} créé par {player.Name}");
    }

    [Command]
    public void CmdJoinLobby(int lobbyId) {
        // Get the player attached to the connection
        Player player = connectionToClient.identity.GetComponent<NetworkPlayer>().PlayerData;
        // Check if the player is already in a lobby
        if (LobbyManager.Instance.IsInLobby(player)) {
            Debug.LogWarning($"{player.Name} est déjà dans un lobby");
            return;
        }
        // Try to join the lobby
        int result = LobbyManager.Instance.JoinLobby(lobbyId, player);
        switch(result) {
            case -1:
                Debug.LogWarning($"{player.Name} est déjà dans ce lobby");
                break;
            case -2:
                Debug.LogWarning($"La partie a déjà commencé dans le lobby {lobbyId}");
                break;
            case -3:
                Debug.LogWarning($"Le lobby {lobbyId} est plein");
                break;
            case -4:
                Debug.LogWarning($"Le lobby {lobbyId} n'existe pas");
                break;
            default:
                Debug.Log($"{player.Name} a rejoint le lobby {lobbyId}");
                break;
        }
    }

    [Command]
    public void CmdLeaveLobby(int lobbyId) {
        // Get the player attached to the connection
        Player player = connectionToClient.identity.GetComponent<NetworkPlayer>().PlayerData;
        // Try to leave the lobby
        int result = LobbyManager.Instance.LeaveLobby(lobbyId, player.Name);
        switch(result) {
            case -1:
                Debug.LogWarning($"{player.Name} n'est pas dans le lobby {lobbyId}");
                break;
            case -2:
                Debug.LogWarning($"Le lobby {lobbyId} n'existe pas");
                break;
            default:
                Debug.Log($"{player.Name} a quitté le lobby {lobbyId}");
                break;
        }
    }

    [Command]
    public void CmdStartGame(int LobbyId) {
        // Get the player attached to the connection
        Player player = connectionToClient.identity.GetComponent<NetworkPlayer>().PlayerData;
        Lobby lobby = LobbyManager.Instance.GetLobbyById(LobbyId);
        // Check if the lobby exists
        if (lobby == null) {
            Debug.LogWarning($"Le lobby {LobbyId} n'existe pas");
            return;
        }
        // Check if the player is the host
        /*if (lobby.HostName != player.Name) {
            Debug.LogWarning($"{player.Name} n'est pas l'hôte du lobby {LobbyId}");
            return;
        }*/
        // Try to start the game
        int result = lobby.StartGame();
        switch(result) {
            case -1:
                Debug.LogWarning($"La partie a déjà commencé dans le lobby {LobbyId}");
                break;
            case -2:
                Debug.LogWarning($"Il n'y a pas assez de joueurs dans le lobby {LobbyId}");
                break;
            case 1:
                AllTargetGameStarted(lobby);
                Debug.Log($"La partie a démarré dans le lobby {LobbyId}");
                AllTargetGameInfo(lobby);
                break;
        }
    }

    // passer en private plus tard
    public void AllTargetGameStarted(Lobby lobby) {
        int nbPlayer = lobby.Players.Count;
        int i = 0;
        foreach (Player player in lobby.Players) {
            if (LobbyManager.Instance.playerConnections.TryGetValue(player.Id, out var conn)) {
                TargetGameStarted(conn.connectionToClient, nbPlayer, i);
                i++;
            }
        }
    }

    [TargetRpc]
    public void TargetGameStarted(NetworkConnectionToClient target, int nbPlayer, int index) {
        Debug.Log($"Vous êtes le joueur {index + 1}/{nbPlayer}");
    }

    [Command]
    public void CmdPlayCard(int cardId) {
        int lobbyId = 1; /// A MODIFIER pour le recuperer automatiquement
        Player player = connectionToClient.identity.GetComponent<NetworkPlayer>().PlayerData;
        Lobby lobby = LobbyManager.Instance.GetLobbyById(lobbyId);
        Card card = player.GetCardById(cardId);
        // Check if the lobby exists
        if (lobby == null) {
            Debug.LogWarning($"Lobby {lobbyId} introuvable");
            return;
        } 
        // Check if the game has started
        if(lobby.GameInstance == null) {
            Debug.LogWarning($"La partie n'a pas encore commencé");
            return;
        }
        // Check if the player has the card
        if (card == null) {
            Debug.LogWarning($"Carte {cardId} introuvable dans la main de {player}");
            return;
        }
        // Try to play the card
        int result = lobby.GameInstance.PlayCard(player, card);
        switch(result) {
            case -1:
                Debug.LogWarning($"{player} n'a pas le droit de jouer pour l'instant");
                break;
            case -2:
                Debug.LogWarning($"{player} n'a pas cette carte en main");
                break;
            case -3:
                Debug.LogWarning($"La carte {card} n'est pas jouable");
                break;
            case -4:
                Debug.LogWarning($"La partie est déja terminée");
                break;
            case 1:
                Debug.Log($"{player} a joué la carte {card}");
                AllTargetGameInfo(lobby);
                break;
        }
    }

    [Command]
    public void CmdDrawCard() {
        int lobbyId = 1; /// A MODIFIER pour le recuperer automatiquement
        // Get the player attached to the connection
        Player player = connectionToClient.identity.GetComponent<NetworkPlayer>().PlayerData;
        Lobby lobby = LobbyManager.Instance.GetLobbyById(lobbyId);
        // Check if the lobby exists
        if (lobby == null) {
            Debug.LogWarning($"Lobby {lobbyId} introuvable");
            return;
        } 
        // Check if the game has started
        if(lobby.GameInstance == null) {
            Debug.LogWarning($"La partie n'a pas encore commencé");
            return;
        }
        // Try to draw a card
        int result = lobby.GameInstance.DrawCard(player);
        switch(result) {
            case -1:
                Debug.LogWarning($"{player} n'a pas le droit de piocher pour l'instant");
                break;
            case -2:
                Debug.LogWarning($"La partie est finie");
                break;
            // ajouter cas pioche vide
            case 1:
                Debug.Log($"{player} a pioché une carte");
                AllTargetGameInfo(lobby);
                break;
        }
    }

    private void AllTargetGameInfo(Lobby lobby) {
        PlayerInfo[] tabPlayers = lobby.GameInstance.GetPlayersAsInfo();
        CardInfo currentCard = lobby.GameInstance.GetCurrentCardAsInfo();
        PlayerInfo currentPlayer = lobby.GameInstance.GetCurrentPlayerAsInfo();
        foreach (Player player in lobby.Players) {
            CardInfo[] handInfo = player.GetHandAsInfo();
            Info gameInfo = new Info();
            gameInfo.Hand = handInfo;
            gameInfo.TabPlayers = tabPlayers;
            gameInfo.CurrentCard = currentCard;
            gameInfo.CurrentPlayer = currentPlayer;
            if (LobbyManager.Instance.playerConnections.TryGetValue(player.Id, out var conn)) {
                TargetGameInfo(conn.connectionToClient, gameInfo);
            }
        }
    }

    [TargetRpc]
    public void TargetGameInfo(NetworkConnectionToClient target, Info info) {
        Debug.Log("TargetGameInfo, first card of hand : " + info.Hand[0].CardC + " " + info.Hand[0].CardV);
    }
}
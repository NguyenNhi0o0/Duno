using Mirror;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerId
{
    public int idMirror;
    public PlayerInfo name;
}

public struct PlayerInfo
{
    public int playerIndex;  // Index du joueur dans la partie
    public string playerName; // Nom du joueur
    public int cardCount;     // Nombre de cartes restantes dans sa main
}

public struct CardInfo
{
    public CardValue cardValue; // Valeur de la carte avec l'enum CardValue
    public CardColor cardColor; // Couleur de la carte avec l'enum CardColor
    public int cardId; // Id de la carte
    public int isPlayable;
}


public class UnoGameManager : NetworkBehaviour
{

    //permet d'utiliser les commande le joueur fait UnoGameManager.Instance.CmdPlayCard(card);
    public static UnoGameManager Instance;
    private void Awake()
    {
        Instance = this;
    }


    // permet de stocker le joueurs ainsi que leur id mirror mais pas sur que ca marche
    private void StoreIdPlayer()
    {
        int i = 0;
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (i >= Players.ConstMaxPlayer)
            {
                Debug.LogError("Erreur : Trop de joueurs connectés");
                return;
            }
            playersId.Add(new PlayerId { idMirror = conn.connectionId, name = new PlayerInfo { playerIndex = i, playerName = "Player" + i } });
            i++;
        }
    }

    private List<PlayerId> playersId = new List<PlayerId>();

    // === 🔹 SYNCVARS === (Synchronisées du serveur vers tous les clients)

    [SyncVar] public int currentPlayerIndex; // Indice du joueur qui doit jouer
    [SyncVar] public int totalPlayers;       // Nombre total de joueurs
    [SyncVar] public List<PlayerInfo> players; // Liste des joueurs avec leurs infos



    // === 🔹 COMMANDES === (Envoyées par un client au serveur)

    /// <summary>
    /// Le joueur envoie une demande au serveur pour jouer une carte.
    /// Si valide, le serveur met à jour l'état du jeu et informe tous les clients.
    /// </summary>
    [Command]
    public void CmdPlayCard(CardInfo card) { }

    /// <summary>
    /// Le joueur envoie une demande au serveur pour piocher une carte.
    /// Le serveur gère la pioche et envoie les informations aux clients concernés.
    /// </summary>
    [Command]
    public void CmdDrawCard() { }

    // === 🔹 CLIENT RPC === (Envoyées par le serveur à tous les clients)

    /// <summary>
    /// Informe tous les clients qu'un joueur a joué une carte.
    /// Met à jour l'affichage de la partie.
    /// </summary>
    [ClientRpc]
    public void RpcPlayerPlayedCard(int playerIndex, CardInfo card)
    {
        Players.playSpecificCard(playerIndex, 0, card.cardValue, card.cardColor);
    }

    /// <summary>
    /// Informe tous les clients qu'un joueur a pioché une carte.
    /// Seul le joueur concerné reçoit la carte exacte via un TargetRpc.
    /// </summary>
    [ClientRpc]
    public void RpcPlayerDrewCard(int playerIndex)
    {
        if (playerIndex == Players.indexMainPlayer)
        {
            return;
        }
        Players.drawCard(playerIndex);
    }

    /// <summary>
    /// Informe tous les clients que la partie est terminé.
    /// envoie aussi l'index du joueur qui a gagné.
    /// </summary>
    [ClientRpc]
    public void EndGame(int playerIndexWinner) { }


    /// <summary>
    /// Informe tous les clients du changement de tour.
    /// Met à jour l'interface utilisateur et la logique du jeu.
    /// </summary>
    [ClientRpc]
    public void RpcEndTurn(int nextPlayerIndex)
    {
        Players.changeTurn(nextPlayerIndex);
    }

    // === 🔹 TARGET RPC === (Envoyées par le serveur à un SEUL client spécifique)

    /// <summary>
    /// Envoie à chaque joueur son propre index dans la partie au début du jeu.
    /// </summary>
    [TargetRpc]
    public void TargetReceivePlayerIndex(NetworkConnectionToClient target, int playerIndex)
    {
        Players.indexMainPlayer = playerIndex;
    }

    /// <summary>
    /// Envoie la carte piochée uniquement au joueur concerné.
    /// Sécurise l'information pour éviter que les autres joueurs ne voient la carte.
    /// </summary>
    [TargetRpc]
    public void TargetReceiveDrawnCard(NetworkConnectionToClient target, CardInfo card)
    {
        Players.drawSpecificCard(Players.indexMainPlayer, card.cardValue, card.cardColor);
    }
}
using Mirror;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    private static int playerCounter = 0;

    [SyncVar]
    public string playerName;

    public Player PlayerData;

    Commands Commands;

    public override void OnStartServer() {
        base.OnStartServer();
        playerCounter++;
        playerName = $"Player#{playerCounter}";
        PlayerData = new Player(playerName);
        LobbyManager.Instance.RegisterPlayer(PlayerData, this);
        Commands = GetComponent<Commands>();
        Commands.CmdCreateLobby();
    }

    public override void OnStartClient(){
        base.OnStartClient();
        Debug.Log($"Client prêt - Joueur : {playerName}");
        if (isLocalPlayer) {
            Commands = GetComponent<Commands>();
            Commands.CmdJoinLobby(1);
        }
    }
}

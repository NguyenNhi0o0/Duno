using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class StartGameUI : MonoBehaviour
{
    public Button startGameButton;
    private NetworkPlayer localPlayer;

    void Start() {
        startGameButton.gameObject.SetActive(false); // on cache au début
        startGameButton.onClick.AddListener(OnStartGameClicked);
    }

    void Update() {
        // On cherche le joueur local (celui avec autorité)
        if (localPlayer == null) {
            foreach (var player in FindObjectsByType<NetworkPlayer>(FindObjectsSortMode.None)) {
                if (player.isLocalPlayer) {
                    localPlayer = player;
                    break;
                }
            }
        }

        // Si le joueur local est prêt, on active le bouton
        if (localPlayer != null && localPlayer.PlayerData != null) {
            Lobby lobby = LobbyManager.Instance.GetLobbyById(1); // provisoire
            if (lobby != null && !lobby.IsGameStarted) {
                startGameButton.gameObject.SetActive(true);
            } else {
                startGameButton.gameObject.SetActive(false);
            }
        }
    }

    void OnStartGameClicked() {
        if (localPlayer != null) {
            localPlayer.GetComponent<Commands>().CmdStartGame(1);
        }
    }
}

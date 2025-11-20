using UnityEngine;
using System.Linq;

public class Players : MonoBehaviour
{
    //Choose who is the real player to follow
    public int indexMainPlayerTmp = 0;
    public static int indexMainPlayer;
    public int numPlayersTmp = 2;
    public static int numPlayers;
    private int maxPlayer = 4;


    void Start()
    {
        numPlayers = numPlayersTmp;
        indexMainPlayer = indexMainPlayerTmp;
        initGame();
    }

    public void initGame()
    {
        if (numPlayers > maxPlayer)
        {
            numPlayers = maxPlayer;
            Debug.LogError("Nb de joueurs max demander trop haut (max" + maxPlayer + ")");
        }
        if (indexMainPlayer >= numPlayers)
        {
            indexMainPlayer = 0;
            Debug.LogError("Index du joueur principal trop haut (max" + numPlayers + ")");
        }

        int playerCount = 0;
        int idInit = 0;
        foreach (Player player in GetComponentsInChildren<Player>())
        {
            player.id = idInit;
            idInit++;
            if (playerCount >= numPlayers)
            {
                player.gameObject.SetActive(false);
            }
            playerCount++;
        }
        InitCam(indexMainPlayer);
    }

    public void InitCam(int id)
    {
        foreach (Player player in GetComponentsInChildren<Player>())
        {
            if (player.id == id)
            {
                player.mainPlayer = true;
                Camera cam = player.GetComponentInChildren<Camera>();
                if (cam != null)
                {
                    cam.gameObject.SetActive(true);
                }
            }
            else
            {
                Camera cam = player.GetComponentInChildren<Camera>();
                if (cam != null)
                {
                    cam.gameObject.SetActive(false);
                }
            }
        }
    }


    public static void playCard(int idPlayer, int idCard)
    {

        Player[] players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);
        Player player = players.FirstOrDefault(p => p.id == idPlayer);

        Transform main = player.transform.Find("Main");
        DispositionCarte script = main.GetComponent<DispositionCarte>();
        script.playCard(idCard);
    }
    public static void playSpecificCard(int idPlayer, int idCard, CardValue cardValue, CardColor cardColor)
    {

        Player[] players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);
        Player player = players.FirstOrDefault(p => p.id == idPlayer);

        Transform main = player.transform.Find("Main");
        DispositionCarte script = main.GetComponent<DispositionCarte>();
        script.playSpecificCard(idCard, cardValue, cardColor);
    }

    public static void drawCard(int idPlayer)
    {
        Player[] players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);
        Player player = players.FirstOrDefault(p => p.id == idPlayer);

        if (player != null)
        {
            Transform main = player.transform.Find("Main");
            DispositionCarte script = main.GetComponent<DispositionCarte>();

            GameObject clone = Instantiate(StaticObject.card);
            clone.SetActive(true);

            // S'assurer que les contraintes sont appliquées immédiatement
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            }

            script.addCard(clone);
        }
        else
        {
            Debug.LogError("Player not found with ID: " + idPlayer);
        }
    }
    public static void drawRandomCard(int idPlayer)
    {
        Player[] players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);
        Player player = players.FirstOrDefault(p => p.id == idPlayer);

        if (player != null)
        {
            Transform main = player.transform.Find("Main");
            DispositionCarte script = main.GetComponent<DispositionCarte>();

            GameObject clone = Instantiate(StaticObject.card);
            clone.SetActive(true);

            // S'assurer que les contraintes sont appliquées immédiatement
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            }
            clone.GetComponent<carte>().setValueAndColor(Serveur.GetRandomCardValue(), Serveur.GetRandomCardColor());
            script.addCard(clone);
        }
        else
        {
            Debug.LogError("Player not found with ID: " + idPlayer);
        }
    }

    public static void drawSpecificCard(int idPlayer, CardValue cardValue, CardColor cardColor)
    {
        Player[] players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);
        Player player = players.FirstOrDefault(p => p.id == idPlayer);

        if (player != null)
        {
            Transform main = player.transform.Find("Main");
            DispositionCarte script = main.GetComponent<DispositionCarte>();

            GameObject clone = Instantiate(StaticObject.card);
            clone.SetActive(true);

            // S'assurer que les contraintes sont appliquées immédiatement
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            }
            clone.GetComponent<carte>().setValueAndColor(cardValue, cardColor);
            script.addCard(clone);
        }
        else
        {
            Debug.LogError("Player not found with ID: " + idPlayer);
        }
    }


    public static void changeTurn(int idPlayer)
    {
        Player[] players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);
        Player player = players.FirstOrDefault(p => p.id == idPlayer);

        if (player != null)
        {
            player.changeTurn();
        }
        else
        {
            Debug.LogError("Player not found with ID: " + idPlayer);
        }
    }

    public static bool isYourTurn()
    {
        Player[] players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);
        Player player = players.FirstOrDefault(p => p.id == indexMainPlayer);
        return player.isYourTurn();
    }

}

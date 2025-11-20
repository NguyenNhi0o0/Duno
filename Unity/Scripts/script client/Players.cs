using UnityEngine;
using System.Linq;



public class Players : MonoBehaviour
{
    public const int ConstMaxPlayer = 4;
    // Indice du joueur principal

    public static int indexMainPlayer;

    // Nombre de joueurs

    public static int numPlayers;


    public static void InitGame()
    {
        Debug.Log("initgame numPlayer" + numPlayers + " indexMainPlayer" + indexMainPlayer);
        if (numPlayers > ConstMaxPlayer)
        {
            numPlayers = ConstMaxPlayer;
            Debug.LogError($"Nombre de joueurs trop élevé (max {ConstMaxPlayer})");
        }
        if (indexMainPlayer >= numPlayers)
        {
            indexMainPlayer = 0;
            Debug.LogError($"Indice du joueur principal trop élevé (max {numPlayers - 1})");
        }

        int playerId = 0;
        foreach (PlayerClient player in StaticObject.players.GetComponentsInChildren<PlayerClient>())
        {
            player.id = playerId++;
            player.gameObject.SetActive(playerId <= numPlayers);
        }
        InitCam(indexMainPlayer);
    }

    private static void InitCam(int id)
    {
        foreach (PlayerClient player in StaticObject.players.GetComponentsInChildren<PlayerClient>())
        {
            bool isMainPlayer = player.id == id;
            player.mainPlayer = isMainPlayer;

            Camera cam = player.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                cam.gameObject.SetActive(isMainPlayer);
            }
        }
    }

    public static void playCard(int idPlayer, int idCard)
    {
        PlayerClient player = FindPlayerById(idPlayer);
        player?.transform.Find("Main").GetComponent<DispositionCard>()?.playCard(idCard);
    }

    public static void playSpecificCard(int idPlayer, int idCard, CardValue cardValue, CardColor cardColor)
    {
        Debug.Log("playSpecificCard");
        PlayerClient player = FindPlayerById(idPlayer);
        player?.transform.Find("Main").GetComponent<DispositionCard>()?.playSpecificCard(idCard, cardValue, cardColor); // idCard c'est la position de la carte dans la main
    }

    public static void drawCard(int idPlayer)
    {
        PlayerClient player = FindPlayerById(idPlayer);
        if (player == null)
        {
            Debug.LogError($"Joueur introuvable avec ID: {idPlayer}");
            return;
        }

        DispositionCard script = player.transform.Find("Main").GetComponent<DispositionCard>();
        GameObject newCard = Instantiate(StaticObject.card);
        newCard.SetActive(true);
        FreezeCardPhysics(newCard);
        script.addCard(newCard);
    }

    public static void DrawMultipleCards(int idPlayer, int nbCards)
    {
        for (int i = 0; i < nbCards; i++)
        {
            drawCard(idPlayer);
        }
    }

    public static void drawRandomCard(int idPlayer)
    {
        PlayerClient player = FindPlayerById(idPlayer);
        if (player == null)
        {
            Debug.LogError($"Joueur introuvable avec ID: {idPlayer}");
            return;
        }

        DispositionCard script = player.transform.Find("Main").GetComponent<DispositionCard>();
        GameObject newCard = Instantiate(StaticObject.card);
        newCard.SetActive(true);
        FreezeCardPhysics(newCard);
        //newCard.GetComponent<Card>().SetValueAndColor(Serveur.GetRandomCardValue(), Serveur.GetRandomCardColor());
        newCard.GetComponent<PlayCard>().SetValueAndColor(CardValue.Wild, CardColor.Black);
        script.addCard(newCard);
    }

    public static void drawSpecificCard(int idPlayer, CardValue cardValue, CardColor cardColor, int idCard)
    {
        PlayerClient player = FindPlayerById(idPlayer);
        if (player == null)
        {
            Debug.LogError($"Joueur introuvable avec ID: {idPlayer}");
            return;
        }

        DispositionCard script = player.transform.Find("Main").GetComponent<DispositionCard>();
        GameObject newCard = Instantiate(StaticObject.card);
        newCard.GetComponent<PlayCard>().idServer = idCard;
        newCard.SetActive(true);
        FreezeCardPhysics(newCard);
        newCard.GetComponent<PlayCard>().SetValueAndColor(cardValue, cardColor);
        script.addCard(newCard);
    }

    public static void changeTurn(int idPlayer)
    {
        PlayerClient[] players = FindObjectsByType<PlayerClient>(FindObjectsSortMode.None);
        foreach (PlayerClient player in players)
        {
            if (player.isYourTurn())
            {
                player.changeTurn();
            }
        }
        PlayerClient playerId = FindPlayerById(idPlayer);
        if (playerId != null)
        {
            playerId.changeTurn();
        }
        else
        {
            Debug.LogError($"Joueur introuvable avec ID: {idPlayer}");
        }
    }

    public static bool isYourTurn()
    {
        PlayerClient player = FindPlayerById(indexMainPlayer);
        return player != null && player.isYourTurn();
    }

    public static PlayerClient FindPlayerById(int idPlayer)
    {
        return Object.FindObjectsByType<PlayerClient>(FindObjectsSortMode.None).FirstOrDefault(p => p.id == idPlayer);
    }

    private static void FreezeCardPhysics(GameObject card)
    {
        Rigidbody rb = card.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
    }
}
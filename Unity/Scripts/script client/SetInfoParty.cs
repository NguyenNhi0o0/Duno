using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class SetInfoParty : MonoBehaviour
{
    private static void DestroyParty()
    {

        PlayCard[] allCards = Object.FindObjectsByType<PlayCard>(FindObjectsSortMode.None); // Trouve tous les scripts Card
        var activeCards = allCards.Where(card => card.gameObject.activeInHierarchy).ToList();
        foreach (var card in activeCards)
        {
            Destroy(card.gameObject);
        }

        DispositionCard[] dispositionsCards = Object.FindObjectsByType<DispositionCard>(FindObjectsSortMode.None);
        var activeDispositionCard = dispositionsCards.Where(disposition => disposition.gameObject.activeInHierarchy).ToList();

        foreach (var disposition in activeDispositionCard)
        {
            disposition.resetMain();
        }

        PlayerClient[] allPlayers = FindObjectsByType<PlayerClient>(FindObjectsSortMode.None);

        foreach (PlayerClient player in allPlayers)
        {
            if (player.isYourTurn())
            {
                player.changeTurn();
            }
        }


        ExpositionCard.setExpositionCard(StaticObject.backCard);
    }

    public static void setCardsPlayers(PlayerInfo[] TabPlayers)
    {

        for (int i = 0; i < TabPlayers.Length; i++)
        {
            if (TabPlayers[i].Index != Players.indexMainPlayer)
                Players.DrawMultipleCards(i, TabPlayers[i].NbCartes);
        }

    }





    public static void setSpecificCardsMainPlayer(CardInfo[] Hand)
    {

        for (int i = 0; i < Hand.Length; i++)
        {
            Players.drawSpecificCard(Players.indexMainPlayer, Hand[i].CardV, Hand[i].CardC, Hand[i].CardId);
        }
    }



    public static void setCurrentPlayer(int index)
    {
        PlayerClient player = Players.FindPlayerById(index);
        player.changeTurn();
    }

    public static void resetAndSetInfoParty(Info info)
    {
        DestroyParty();
        setCardsPlayers(info.TabPlayers);
        setSpecificCardsMainPlayer(info.Hand);

        setCurrentPlayer(info.CurrentPlayer.Index);
        ExpositionCard.setExpositionCardValueColor(info.CurrentCard.CardV, info.CurrentCard.CardC);
    }



    // create a random info
    private Info randomInfo()
    {
        Info info = new Info();
        int nbPlayers = Random.Range(0, 4);
        info.TabPlayers = new PlayerInfo[nbPlayers];
        for (int i = 0; i < nbPlayers; i++)
        {
            info.TabPlayers[i] = new PlayerInfo { Index = i, NbCartes = Random.Range(5, 11) };
        }
        info.Hand = new CardInfo[7];
        for (int i = 0; i < 7; i++)
        {
            info.Hand[i] = new CardInfo { CardC = (CardColor)Random.Range(0, 4), CardV = (CardValue)Random.Range(0, 15), CardId = i };
        }
        info.CurrentCard = new CardInfo { CardC = (CardColor)Random.Range(0, 4), CardV = (CardValue)Random.Range(0, 15), CardId = 0 };
        info.CurrentPlayer = new PlayerInfo { Index = Random.Range(0, nbPlayers), NbCartes = Random.Range(5, 11) };
        return info;

    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Info info = new Info();
            info = randomInfo();
            /*
            info.Hand = new CardInfo[3];
            info.Hand[0] = new CardInfo { CardC = CardColor.Red, CardV = CardValue.One, CardId = 1 };
            info.Hand[1] = new CardInfo { CardC = CardColor.Blue, CardV = CardValue.Two, CardId = 2 };
            info.Hand[2] = new CardInfo { CardC = CardColor.Yellow, CardV = CardValue.Three, CardId = 3 };
            info.TabPlayers = new PlayerInfo[3];
            info.TabPlayers[0] = new PlayerInfo { Index = 0, NbCartes = 7 };
            info.TabPlayers[1] = new PlayerInfo { Index = 1, NbCartes = 7 };
            info.TabPlayers[2] = new PlayerInfo { Index = 2, NbCartes = 7 };
            info.CurrentCard = new CardInfo { CardC = CardColor.Red, CardV = CardValue.Zero, CardId = 0 };
            info.CurrentPlayer = new PlayerInfo { Index = 0, NbCartes = 7 };
            */
            resetAndSetInfoParty(info);
        }
    }
}

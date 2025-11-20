using UnityEngine;

public class InitGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int nbPlayers = 4;
    public int indexMainPlayer = 0;

    public int[] idPlayers = { 1, 2, 3 };
    public int[] nbCards = { 7, 7, 7 };

    public int idPlayer = 0;
    public int nbCard = 7;
    public CardValue[] cardValues = { CardValue.Zero, CardValue.One, CardValue.Two, CardValue.Three, CardValue.Four, CardValue.Five, CardValue.Six };
    public CardColor[] cardColors = { CardColor.Red, CardColor.Blue, CardColor.Green, CardColor.Yellow, CardColor.Red, CardColor.Blue, CardColor.Green };
    void Start()
    {
        Players.numPlayers = nbPlayers;
        Players.indexMainPlayer = indexMainPlayer;
        Players.InitGame();
        SetInfoParty.setCardsPlayers(idPlayers, nbCards);
        SetInfoParty.setSpecificCardsPlayer(idPlayer, nbCard, cardValues, cardColors);
    }
}

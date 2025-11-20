using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class SetInfoParty : MonoBehaviour
{
    private void DestroyParty()
    {

        Card[] allCards = Object.FindObjectsByType<Card>(FindObjectsSortMode.None); // Trouve tous les scripts Card
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


        StaticObject.expositionCard.GetComponent<ExpositionCard>().setExpositionCard(StaticObject.backCard);
    }

    private void setCardsPlayers(int[] idPlayer, int[] nbCard)
    {
        if (idPlayer.Length != nbCard.Length)
        {
            Debug.LogError("Les tableaux idPlayer et nbCard doivent avoir la même taille");
            return;
        }
        for (int i = 0; i < idPlayer.Length; i++)
        {
            Players.DrawMultipleCards(idPlayer[i], nbCard[i]);
        }

    }

    private void setSpecificCardsPlayer(int idPlayer, int nbCard, CardValue[] cardValues, CardColor[] cardColors)
    {
        if (cardValues.Length != cardColors.Length)
        {
            Debug.LogError("Les tableaux cardValues et cardColors doivent avoir la même taille");
            return;
        }
        if (cardValues.Length != nbCard)
        {
            Debug.LogError("Le tableau nbCard doit avoir la même taille que les tableaux cardValues et cardColors");
            return;
        }
        for (int i = 0; i < nbCard; i++)
        {
            Players.drawSpecificCard(idPlayer, cardValues[i], cardColors[i]);
        }

    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyParty();
            setCardsPlayers(new int[] { 1, 2, 3 }, new int[] { 7, 7, 7 });
            setSpecificCardsPlayer(0, 3, new CardValue[] { CardValue.One, CardValue.Two, CardValue.Three }, new CardColor[] { CardColor.Red, CardColor.Blue, CardColor.Yellow });
        }
    }
}

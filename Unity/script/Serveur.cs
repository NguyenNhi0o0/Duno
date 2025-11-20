using UnityEngine;
using System.Collections;

public class Serveur : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static CardValue GetRandomCardValue()
    {
        // Créer un tableau des valeurs autorisées (sans wild et plusFour)
        CardValue[] allowedValues = new CardValue[]
        {
            CardValue.zero,
            CardValue.one,
            CardValue.two,
            CardValue.three,
            CardValue.four,
            CardValue.five,
            CardValue.six,
            CardValue.seven,
            CardValue.eight,
            CardValue.nine,
            CardValue.plusTwo,
            CardValue.skip,
            CardValue.reverse
        };

        // Sélectionner un index aléatoire
        int randomIndex = UnityEngine.Random.Range(0, allowedValues.Length);

        // Retourner la valeur aléatoire
        return allowedValues[randomIndex];
    }

    public static CardColor GetRandomCardColor()
    {
        // Créer un tableau des couleurs autorisées (sans multiColor)
        CardColor[] allowedColors = new CardColor[]
        {
            CardColor.red,
            CardColor.blue,
            CardColor.green,
            CardColor.yellow
        };

        // Sélectionner un index aléatoire
        int randomIndex = UnityEngine.Random.Range(0, allowedColors.Length);

        // Retourner la couleur aléatoire
        return allowedColors[randomIndex];
    }

    void Start()
    {
        // Démarre la coroutine
        StartCoroutine(RepeatAction());
        //Players.changeTurn(Players.indexMainPlayer);
    }

    IEnumerator RepeatAction()
    {
        int max = Players.numPlayers;

        int playerActualy = Players.indexMainPlayer;
        Clock.setfullRotationDuration(5.0f);
        while (true)
        {
            Clock.staticStartRotation();
            Players.changeTurn(playerActualy);
            if (playerActualy != Players.indexMainPlayer)
            {
                yield return new WaitForSeconds(2.5f);
                Players.drawCard(playerActualy);
                //Players.drawSpecificCard(playerActualy, GetRandomCardValue(), GetRandomCardColor());
                yield return new WaitForSeconds(2.5f);
                //Players.playCard(playerActualy, 0);
                Players.playSpecificCard(playerActualy, 0, GetRandomCardValue(), GetRandomCardColor());
            }
            else
            {
                yield return new WaitForSeconds(5);
            }
            Players.changeTurn(playerActualy);

            playerActualy = (playerActualy + 1) % max;
        }
    }
}

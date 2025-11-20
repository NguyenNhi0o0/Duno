using UnityEngine;
using System.Linq;

public class DrawPile : MonoBehaviour
{
    public Players players;

    public GameObject card;

    private int idPlayer;
    public bool jePeutPiocher = true;
    private Transform main;


    void OnMouseDown()
    {
        if (jePeutPiocher && Players.isYourTurn())
        {
            idPlayer = Players.indexMainPlayer;
            Players.drawRandomCard(idPlayer);
        }
    }
}

using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    public int id;
    public bool mainPlayer = false;

    private bool yourTurn = false;

    void Start()
    {
        if (mainPlayer)
        {
            yourTurn = true;
            Candle candle = GetComponentInChildren<Candle>();
            candle.ActivateAllChildren();
        }
        else
        {
            yourTurn = false;
            Candle candle = GetComponentInChildren<Candle>();
            candle.DeactivateAllChildren();
        }
    }

    public bool isYourTurn()
    {
        return yourTurn;
    }

    public void changeTurn()
    {
        Candle candle = GetComponentInChildren<Candle>();
        if (!yourTurn)
        {
            candle.ActivateAllChildren();
            if (mainPlayer)
            {
                //NetworkBehaviourFinal.Instance.CmdChangeCardPlayable(); // actualise la main du joueur si on peut la jouer
            }
        }
        else
            candle.DeactivateAllChildren();
        yourTurn = !yourTurn;
    }
}
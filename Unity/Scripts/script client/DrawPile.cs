using UnityEngine;
using System.Linq;

public class DrawPile : MonoBehaviour
{

    public static bool CanDraw = true;
    private Transform main;

    private void OnMouseDown()
    {
        testSpecif.Instance.CmdDrawCard();
        /*
        if (CanDraw && Players.isYourTurn())
        {

            //NetworkBehaviourFinal.Instance.CmdDrawCard(NetworkBehaviourFinal.Instance.idLobby);
            CanDraw = false;

            //Players.drawRandomCard(Players.indexMainPlayer);
        }
        */
    }
}

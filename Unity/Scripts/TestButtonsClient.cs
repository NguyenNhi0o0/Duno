using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TestButtons : MonoBehaviour
{
    public GameObject buttonPrefab; // Assigne le prefab du bouton
    public Transform buttonParent;  // Assigne le Panel contenant les boutons
    public string[] buttonTexts = { "CmdDrawCard", "CmdPlayCard" };

    private void Start()
    {
        foreach (string text in buttonTexts)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            buttonObj.GetComponentInChildren<Text>().text = text; // Utilise le Text (Legacy)
            buttonObj.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(text));
        }
    }

    private void ButtonClicked(string buttonText)
    {
        switch (buttonText)
        {
            case "CmdDrawCard":
                //NetworkBehaviourFinal.Instance.RpcPlayerPlayedCard(0, new CardInfo { cardValue = CardValue.Two, cardColor = CardColor.Red, cardId = 0, isPlayable = 1 });
                break;
            case "CmdPlayCard":
                //NetworkBehaviourFinal.Instance.RpcPlayerDrewCard(0);
                break;
            default:
                Debug.LogError("Button text not found: " + buttonText);
                break;
        }
    }
}

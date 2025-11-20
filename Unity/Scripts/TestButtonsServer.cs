using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TestButtonsServer : MonoBehaviour
{
    public GameObject buttonPrefab; // Assigne le prefab du bouton
    public Transform buttonParent;  // Assigne le Panel contenant les boutons
    public string[] buttonTexts = { "RpcPlayerPlayedCard", "RpcPlayerDrewCard", "EndGame", "RpcEndTurn" };

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
            case "RpcPlayerPlayedCard":
                //NetworkBehaviourFinal.Instance.RpcPlayerPlayedCard(0, new CardInfo { cardValue = CardValue.Two, cardColor = CardColor.Red, cardId = 0, isPlayable = 1 });
                break;
            case "RpcPlayerDrewCard":
                //NetworkBehaviourFinal.Instance.RpcPlayerDrewCard(0);
                break;
            case "EndGame":
                //NetworkBehaviourFinal.Instance.EndGame(0);
                break;
            case "RpcEndTurn":
                //NetworkBehaviourFinal.Instance.RpcEndTurn(0);
                break;
            default:
                Debug.LogError("Button text not found: " + buttonText);
                break;
        }
    }
}

using UnityEngine;

public class Candle : MonoBehaviour
{
    // Méthode pour activer tous les enfants
    public void ActivateAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    // Méthode pour désactiver tous les enfants
    public void DeactivateAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}

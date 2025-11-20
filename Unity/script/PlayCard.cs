using UnityEngine;

public class PlayCard : MonoBehaviour
{
    private Rigidbody rb;
    private DispositionCarte dispositionCarte;

    public bool isIt = true;
    public int id;


    void Start()
    {
        // Obtenir le composant DispositionCarte du parent
        dispositionCarte = transform.parent.GetComponent<DispositionCarte>();
        rb = GetComponent<Rigidbody>();

        // S'assurer que la carte ne bouge pas initialement
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    void OnMouseDown()
    {

        if (isIt)
        {
            if (transform.parent.parent.GetComponent<Player>().isYourTurn())
            {
                isIt = false;


                // Appeler playCard avec l'ID de la carte
                dispositionCarte.playCard(id);
            }

        }
    }

}

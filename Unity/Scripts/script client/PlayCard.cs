using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class PlayCard : MonoBehaviour
{
    private Rigidbody rb;
    private DispositionCard dispositionCard;
    public bool isIt = true;
    public CardColor Color;
    public CardValue Value;
    public static bool GravityInverted = false;
    public bool IsShine { get; set; } = false;
    public Renderer CardRenderer;
    public int id;
    public int idServer;


    public void SetValueAndColor(CardValue value, CardColor color)
    {
        Value = value;
        Color = color;
        GetComponent<Renderer>().material = StaticObject.ConvertCardNameToMaterial(value, color);
    }
    public void SetValue(CardValue value)
    {
        Value = value;
        GetComponent<Renderer>().material = StaticObject.ConvertCardNameToMaterial(value, Color);
    }
    public void SetColor(CardColor color)
    {
        Color = color;
        GetComponent<Renderer>().material = StaticObject.ConvertCardNameToMaterial(Value, color);
    }


    public void changeisIt(bool isItBool)
    {
        isIt = isItBool;
        PlayCard card = GetComponent<PlayCard>();
        //card.IsShine = isItbool;
        if (isItBool)
            card.CardRenderer.material.EnableKeyword("_EMISSION");
        else
            card.CardRenderer.material.DisableKeyword("_EMISSION");
    }

    void Start()
    {
        // Obtenir le composant DispositionCard du parent
        dispositionCard = transform.parent.GetComponent<DispositionCard>();
        rb = GetComponent<Rigidbody>();

        // S'assurer que la carte ne bouge pas initialement
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;


        CardRenderer = GetComponent<Renderer>();
        CardRenderer.material = new Material(CardRenderer.material); // Crée une copie du matériau
        CardRenderer.material.EnableKeyword("_EMISSION");
        Color emissionColor = new Color(1f, 0f, 0f); // Rouge
        CardRenderer.material.SetColor("_EmissionColor", emissionColor);
        CardRenderer.material.DisableKeyword("_EMISSION");
    }


    public IEnumerator SelectColor(int index)
    {
        DispositionCard dispositionCard = GetComponentInParent<DispositionCard>();

        Transform card = dispositionCard.transformsList[index];
        card.GetComponent<PlayCard>().isIt = false;

        card.position = new Vector3(card.position.x, card.position.y + 0.4f, card.position.z);
        card.rotation = Quaternion.Euler(0, 0, 0);
        StaticObject.CardMultiColorLight.SetActive(true);
        StaticObject.CardMultiColorLight.transform.position = card.position;

        DispositionCard.multiCardIsPlayed = true;
        yield return new WaitUntil(() => DispositionCard.colorIsChosen == true);
        DispositionCard.colorIsChosen = false;
        DispositionCard.multiCardIsPlayed = false;

        card.GetComponent<PlayCard>().SetColor(DispositionCard.colorChosen);
        dispositionCard.playCardV2(id);
    }



    void OnMouseDown()
    {

        testSpecif.Instance.CmdPlayCard(idServer);
        /*
        if (DispositionCard.multiCardIsPlayed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // Vérifie si on touche un objet
            {
                if (hit.collider.gameObject == gameObject) // Vérifie si c'est bien la carte
                {
                    DetermineZone(hit.point);
                }
            }
        }
        else if (isIt)
        {
            if (transform.parent.parent.GetComponent<Player>().isYourTurn())
            {
                isIt = false;

                DispositionCard dispositionCard = GetComponentInParent<DispositionCard>();
                Transform card = dispositionCard.transformsList[id];
                if (card.GetComponent<Card>().Value == CardValue.Wild || card.GetComponent<Card>().Value == CardValue.WildDrawFour)
                {
                    StartCoroutine(SelectColor(id));
                }
                else
                {
                    dispositionCard.playCardV2(id);
                }
            }
        }
        */
    }

    void DetermineZone(Vector3 clickPos)
    {
        Bounds bounds = GetComponent<Collider>().bounds;
        Vector3 center = bounds.center;

        float width = bounds.size.x / 2f; // Moitié de la largeur
        float height = bounds.size.y / 2f; // Moitié de la hauteur

        // Comparer avec le centre pour savoir dans quel quadrant on est
        if (clickPos.x >= center.x && clickPos.y >= center.y)
        {
            //NetworkBehaviourFinal.Instance.CmdChooseColor(NetworkBehaviourFinal.Instance.idLobby, CardColor.Blue);
            DispositionCard.colorIsChosen = true;
        }

        else if (clickPos.x < center.x && clickPos.y >= center.y)
        {
            DispositionCard.colorIsChosen = true;
            //NetworkBehaviourFinal.Instance.CmdChooseColor(NetworkBehaviourFinal.Instance.idLobby, CardColor.Red);
        }


        else if (clickPos.x < center.x && clickPos.y < center.y)
        {
            DispositionCard.colorIsChosen = true;
            //NetworkBehaviourFinal.Instance.CmdChooseColor(NetworkBehaviourFinal.Instance.idLobby, CardColor.Yellow);
        }


        else
        {
            DispositionCard.colorIsChosen = true;
            //NetworkBehaviourFinal.Instance.CmdChooseColor(NetworkBehaviourFinal.Instance.idLobby, CardColor.Green);
        }


    }



    private void Update()
    {

        if (transform.parent != null && transform.parent.name == "Tas")
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
        }
        //bourin mais fonctionne
    }

    /*
    private void FixedUpdate() // Utiliser FixedUpdate pour les Rigidbodies
    {
        CardRigidbody.useGravity = !GravityInverted; // Active/Désactive la gravité Unity

        if (GravityInverted)
        {
            CardRigidbody.AddForce(Vector3.up * 0.5f, ForceMode.Acceleration); // Applique une force inverse en continu
        }
    }
    */
}

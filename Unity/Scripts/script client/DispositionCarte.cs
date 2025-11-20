using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class DispositionCard : MonoBehaviour
{
    public static GameObject tas;
    private float nbCard = 0;

    public List<Transform> transformsList = new List<Transform>();

    public static bool multiCardIsPlayed = false;
    public static bool colorIsChosen = false;
    public static CardColor colorChosen = CardColor.Black;



    public void resetMain()
    {
        transformsList.Clear();
        nbCard = 0;
    }
    public static void SetColorChosen(CardColor color)
    {
        colorChosen = color;
        colorIsChosen = true;
    }

    public static void tpCarte(Transform card)
    {
        card.transform.SetParent(StaticObject.tas.transform);
        float x = Random.Range(-0.3f, 0.3f);
        float y = Random.Range(-0.3f, 0.3f);
        float z = Random.Range(-0.3f, 0.3f);
        card.transform.localPosition = new Vector3(x, y, z);
        x = Random.Range(45f, 135f);
        y = Random.Range(0f, 180f);
        z = Random.Range(0f, 180f);
        card.transform.rotation = Quaternion.Euler(x, y, z);

        // Activer la gravité pour que la carte tombe
        Rigidbody rb = card.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void addCard(GameObject card)
    {
        nbCard++;
        transformsList.Add(card.transform);
        card.transform.SetParent(transform);

        // Figer les rotations pour empêcher la carte de tourner
        Rigidbody rb = card.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }

        // Mettre à jour les IDs des cartes pour correspondre à leur position dans la liste
        updateCardIDs();

        // Calcul de l'offset (décalage entre les cartes)
        float cardSpacing = 0.1f;
        float halfWidth = cardSpacing * (nbCard - 1) / 2f;

        // Applique les positions des cartes de façon correcte même si le joueur tourne
        for (int i = 0; i < transformsList.Count; i++)
        {
            Transform currentCard = transformsList[i];
            float localOffset = cardSpacing * i - halfWidth;

            // Positionner en local, mais tenir compte de la rotation du joueur
            Vector3 localPosition;

            // Si le joueur est tourné de 90 degrés, aligner les cartes le long de l'axe Z
            if (Mathf.Abs(transform.rotation.eulerAngles.y) % 180 == 90)
            {
                localPosition = new Vector3(0, 0, localOffset);
            }
            else
            {
                localPosition = new Vector3(localOffset, 0, 0);
            }

            // Appliquer la position relative à la main du joueur
            currentCard.localPosition = localPosition;
        }

        // Centrer le parent (joueur) après l'ajout de la carte
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // Appliquer la rotation correcte à la carte
        card.transform.rotation = transform.rotation * Quaternion.Euler(0, -10, 0);
    }


    IEnumerator playCardCoV2(int id)
    {

        if (nbCard == 0)
        {
            yield break;
        }
        // Trouver l'index de la carte à supprimer
        int index = transformsList.FindIndex(t => t.GetComponent<PlayCard>().id == id);

        if (index == -1)
        {
            Debug.LogError("Aucune carte trouvée avec l'ID : " + id);
            yield break;
        }

        Transform card = transformsList[index];
        card.GetComponent<PlayCard>().isIt = false;
        //StaticObject.expositionCard.GetComponent<Renderer>().material = card.GetComponent<Renderer>().material;

        ExpositionCard.setExpositionCard(card.GetComponent<Renderer>().material);


        // Mettre à jour les IDs des cartes pour correspondre à leur position dans la liste
        updateCardIDs();

        // Déplacer la carte vers le tas
        //tpCarte(cardToRemove);
        // supprimer

        PlayCard cardComp = card.GetComponent<PlayCard>();
        //NetworkBehaviourFinal.Instance.CmdPlayCard(NetworkBehaviourFinal.Instance.idLobby, this.gameObject.GetComponentPlayCard().idServer);

        //NetworkBehaviourFinal.Instance.CmdPlayCard(new CardInfo { cardValue = cardComp.Value, cardColor = cardComp.Color, cardId = cardComp.id, isPlayable = 0 });
        removeCardHand(index);
    }




    public void removeCardHand(int index)
    {
        Transform cardToRemove = transformsList[index];
        transformsList.RemoveAt(index);
        updateCardIDs();
        PlayCard cardComp = cardToRemove.GetComponent<PlayCard>();
        Destroy(cardToRemove.gameObject);

        nbCard--;

        // Ajuster la position de toutes les cartes restantes
        float cardSpacing = 0.1f;
        float halfWidth = cardSpacing * (transformsList.Count - 1) / 2f;

        for (int i = 0; i < transformsList.Count; i++)
        {
            Transform currentCard = transformsList[i];
            float localOffset = cardSpacing * i - halfWidth;

            // Positionner en local, mais tenir compte de la rotation du joueur
            Vector3 localPosition;

            // Si le joueur est tourné de 90 degrés, aligner les cartes le long de l'axe Z
            if (Mathf.Abs(transform.rotation.eulerAngles.y) % 180 == 90)
            {
                localPosition = new Vector3(0, 0, localOffset);
            }
            else
            {
                localPosition = new Vector3(localOffset, 0, 0);
            }

            // Appliquer la position relative à la main du joueur
            currentCard.localPosition = localPosition;
        }

        // Centrer le parent (joueur) après l'ajout de la carte
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }


    public void playCard(int id)
    {

        StartCoroutine(playCardCoV2(id));
    }
    public void playCardV2(int id)
    {

        StartCoroutine(playCardCoV2(id));
    }

    public void playSpecificCard(int id, CardValue cardValue, CardColor cardColor)
    {

        //int index = transformsList.FindIndex(t => t.GetComponent<PlayCard>().id == id);
        //Transform card = transformsList[index];
        //card.GetComponentPlayCard().SetValueAndColor(cardValue, cardColor);

        GameObject NewCard = Instantiate(StaticObject.card);
        NewCard.SetActive(true);
        NewCard.GetComponent<PlayCard>().SetValueAndColor(cardValue, cardColor);
        Rigidbody rb = NewCard.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        tpCarte(NewCard.transform);

        removeCardHand(id);

        //StartCoroutine(playCardCoV2(id));
    }

    private void updateCardIDs()
    {
        for (int i = 0; i < transformsList.Count; i++)
        {
            transformsList[i].GetComponent<PlayCard>().id = i;
        }
    }
}
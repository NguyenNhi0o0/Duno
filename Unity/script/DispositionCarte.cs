using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class DispositionCarte : MonoBehaviour
{
    public GameObject tas;
    private float nbCard = 0;

    public List<Transform> transformsList = new List<Transform>();

    public static bool multiCardIsPlayed = false;
    private static bool colorIsChosen = false;
    private static CardColor colorChosen = CardColor.multiColor;

    public static void SetColorChosen(CardColor color)
    {
        colorChosen = color;
        colorIsChosen = true;
    }

    void tpCarte(Transform card)
    {
        card.transform.SetParent(tas.transform);
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



    IEnumerator playCardCo(int id)
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


        if (card.GetComponent<carte>().value == CardValue.wild || card.GetComponent<carte>().value == CardValue.plusFour)
        {
            card.position = new Vector3(card.position.x, card.position.y + 0.4f, card.position.z);
            card.rotation = Quaternion.Euler(0, 0, 0);
            StaticObject.CardMultiColorLight.SetActive(true);
            StaticObject.CardMultiColorLight.transform.position = card.position;

            multiCardIsPlayed = true;
            yield return new WaitUntil(() => colorIsChosen == true);
            colorIsChosen = false;
            multiCardIsPlayed = false;
            switch (colorChosen)
            {
                case CardColor.red:
                    card.GetComponent<carte>().color = CardColor.red;
                    card.GetComponent<Renderer>().material = StaticObject.colorRed;
                    break;
                case CardColor.blue:
                    card.GetComponent<carte>().color = CardColor.blue;
                    card.GetComponent<Renderer>().material = StaticObject.colorBlue;
                    break;
                case CardColor.green:
                    card.GetComponent<carte>().color = CardColor.green;
                    card.GetComponent<Renderer>().material = StaticObject.colorGreen;
                    break;
                case CardColor.yellow:
                    card.GetComponent<carte>().color = CardColor.yellow;
                    card.GetComponent<Renderer>().material = StaticObject.colorYellow;
                    break;
            }

        }
        //StaticObject.expositionCard.GetComponent<Renderer>().material = card.GetComponent<Renderer>().material;

        Material newMat = new Material(card.GetComponent<Renderer>().material);
        newMat.SetColor("_Color", new Color(1, 1, 1, 0.25f));
        newMat.shader = Shader.Find("Custom/HologramShader");

        Material[] mat = new Material[2];
        mat[0] = newMat;
        mat[1] = newMat;
        StaticObject.expositionCard.GetComponent<Renderer>().materials = mat;



        card.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        card.gameObject.GetComponent<Rigidbody>().useGravity = true;

        // Supprimer l'élément de la liste
        Transform cardToRemove = transformsList[index];
        transformsList.RemoveAt(index);

        // Mettre à jour les IDs des cartes pour correspondre à leur position dans la liste
        updateCardIDs();

        // Déplacer la carte vers le tas
        tpCarte(cardToRemove);

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
        StartCoroutine(playCardCo(id));
    }

    public void playSpecificCard(int id, CardValue cardValue, CardColor cardColor)
    {
        int index = transformsList.FindIndex(t => t.GetComponent<PlayCard>().id == id);
        Transform card = transformsList[index];
        card.GetComponent<carte>().setValueAndColor(cardValue, cardColor);
        StartCoroutine(playCardCo(id));
    }

    private void updateCardIDs()
    {
        for (int i = 0; i < transformsList.Count; i++)
        {
            transformsList[i].GetComponent<PlayCard>().id = i;
        }
    }


}

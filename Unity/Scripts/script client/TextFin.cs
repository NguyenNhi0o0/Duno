using UnityEngine;
using TMPro;


public class TextFin : MonoBehaviour
{
    public static float rotationSpeed = 30.0f;

    public void Fin(bool win)
    {
        gameObject.SetActive(true);
        if (win)
            GetComponent<TextMeshPro>().text = "Victoire";
        else
            GetComponent<TextMeshPro>().text = "Défaite";

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

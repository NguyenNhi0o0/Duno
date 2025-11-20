using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject pointerSeconds; // Aiguille des secondes
    public static float fullRotationDuration = 3.0f; // Durée en secondes pour un tour complet
    private float rotationProgress = 0.0f;
    private bool isRotating = false;

    void Start()
    {
        //StartRotation();
    }

    void Update()
    {
        if (isRotating)
        {
            // Augmenter la progression de la rotation
            rotationProgress += Time.deltaTime / fullRotationDuration;

            // Calculer l'angle de rotation
            float rotationAngle = Mathf.Lerp(0, 360, rotationProgress);

            // Appliquer la rotation à l'aiguille
            pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationAngle);

            // Arrêter la rotation après un tour complet
            if (rotationProgress >= 1.0f)
            {
                isRotating = false;
                rotationProgress = 0.0f;
            }
        }
    }

    // Fonction pour lancer la rotation
    public void StartRotation()
    {

        pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        rotationProgress = 0.0f;
        isRotating = true;
    }


    public static void staticStartRotation()
    {
        Clock[] clocks = Object.FindObjectsByType<Clock>(FindObjectsSortMode.None);

        foreach (var clock in clocks)
        {
            clock.StartRotation();
        }
    }


    public static void setfullRotationDuration(float duration)
    {
        fullRotationDuration = duration;
    }
}
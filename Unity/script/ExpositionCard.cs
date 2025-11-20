using UnityEngine;

public class ExpositionCard : MonoBehaviour

{
    // Vitesse de rotation (degrés par seconde)
    public float rotationSpeed = 30.0f;

    void Update()
    {
        // Faire tourner l'objet autour de l'axe Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


}


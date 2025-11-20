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

    public static void setExpositionCardValueColor(CardValue value, CardColor color)
    {
        setExpositionCard(StaticObject.ConvertCardNameToMaterial(value, color));
    }

    public static void setExpositionCard(Material Mat)
    {
        Material newMat = new Material(Mat);
        newMat.SetColor("_Color", new Color(1, 1, 1, 0.25f));
        newMat.shader = Shader.Find("Custom/HologramShader");

        Material[] mat = new Material[2];
        mat[0] = newMat;
        mat[1] = newMat;
        StaticObject.expositionCard.GetComponent<Renderer>().materials = mat;
    }


}


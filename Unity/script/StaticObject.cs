using UnityEngine;

public enum CardColor
{
    red,
    blue,
    green,
    yellow,
    multiColor
}


public enum CardValue
{
    zero,
    one,
    two,
    three,
    four,
    five,
    six,
    seven,
    eight,
    nine,
    plusTwo,
    skip,
    reverse,
    plusFour,
    wild
}


public class StaticObject : MonoBehaviour
{
    public static GameObject card;
    public GameObject tempCard;

    public static GameObject CardMultiColorLight;
    public GameObject tempCardMultiColorLight;

    public static Material colorRed;
    public Material tempColorRed;

    public static Material colorBlue;
    public Material tempColorBlue;

    public static Material colorGreen;
    public Material tempColorGreen;

    public static Material colorYellow;
    public Material tempColorYellow;

    public static GameObject expositionCard;
    public GameObject tempExpositionCard;



    void Start()
    {
        card = tempCard;
        CardMultiColorLight = tempCardMultiColorLight;
        colorRed = tempColorRed;
        colorBlue = tempColorBlue;
        colorGreen = tempColorGreen;
        colorYellow = tempColorYellow;
        expositionCard = tempExpositionCard;
    }


    public static Material ConvertCardNameToMaterial(CardValue value, CardColor color)
    {
        string path = value + "_" + color;
        Material material = Resources.Load<Material>(path);
        if (material == null)
        {
            Debug.LogError("Aucun matériau trouvé à l'emplacement : " + path);
            return null;
        }
        return material;
    }
}
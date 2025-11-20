using UnityEngine;

public enum CardColor { Red, Green, Blue, Yellow, Black }
public enum CardValue { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Skip, Reverse, DrawTwo, Wild, WildDrawFour }


public class StaticObject : MonoBehaviour
{
    public static GameObject card;
    [SerializeField] private GameObject tempCard;

    public static GameObject CardMultiColorLight;
    [SerializeField] private GameObject tempCardMultiColorLight;

    public static GameObject expositionCard;
    [SerializeField] private GameObject tempExpositionCard;

    public static Material backCard;
    [SerializeField] private Material tempBackCard;

    public static Players players;
    [SerializeField] private Players tempPlayers;

    public static GameObject tas;
    [SerializeField] private GameObject tempTas;

    public static TextFin textFin;
    [SerializeField] private TextFin tempTextFin;

    void Awake()
    {
        card = tempCard;
        CardMultiColorLight = tempCardMultiColorLight;
        expositionCard = tempExpositionCard;
        backCard = tempBackCard;
        players = tempPlayers;
        tas = tempTas;
        textFin = tempTextFin;
    }


    public static Material ConvertCardNameToMaterial(CardValue value, CardColor color)
    {
        string path = $"CardMaterials/{value}_{color}";
        Material material = Resources.Load<Material>(path);
        if (material == null)
        {
            Debug.LogError($"Aucun matériau trouvé à l'emplacement : {path}");
            return null;
        }
        return material;
    }
}
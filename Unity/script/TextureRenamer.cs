using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureRenamer : EditorWindow
{
    private static readonly string[] colorOrder = { "yellow", "red", "green", "blue" };

    private const string multiColor = "multicolor";
    private static string texturePath = "Assets/Card/Textures"; // Change ici le chemin

    // Énumération des valeurs de carte
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
        wild,
        back
    }

    [MenuItem("Tools/Rename Textures")]
    public static void RenameTextures()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture", new[] { texturePath });

        if (guids.Length != 55)
        {
            Debug.LogError($"Erreur: Il faut exactement 55 textures, mais {guids.Length} trouvées !");
            return;
        }

        int index = 0;

        // Renommer les 52 cartes colorées
        for (int i = 0; i < 52; i++)
        {
            string color = colorOrder[i % 4]; // Récupère la couleur
            CardValue cardValue = (CardValue)(i / 4); // Calcule la valeur de la carte

            RenameTexture(guids[index++], $"{cardValue}_{color}");
        }

        // Les cartes spéciales multicolor
        RenameTexture(guids[index++], $"{CardValue.plusFour}_{multiColor}");
        RenameTexture(guids[index++], $"{CardValue.wild}_{multiColor}");
        RenameTexture(guids[index++], $"{CardValue.back}_{multiColor}");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Renommage terminé !");
    }

    private static void RenameTexture(string guid, string newName)
    {
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        string newPath = Path.Combine(Path.GetDirectoryName(assetPath), newName + Path.GetExtension(assetPath));

        AssetDatabase.RenameAsset(assetPath, newName);
        Debug.Log($"Renommé: {assetPath} -> {newName}");
    }
}

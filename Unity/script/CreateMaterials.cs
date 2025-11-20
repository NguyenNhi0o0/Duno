using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAndConvertMaterialsToURP : MonoBehaviour
{
    [MenuItem("Tools/Create and Convert Materials to URP")]
    static void CreateAndConvertMaterials()
    {
        // Chemins des textures et des matériaux
        string texturesPath = "Assets/Card/Textures";
        string materialsPath = "Assets/Card/Materials";
        string holoMaterialsPath = "Assets/Card/HoloMaterials"; // Chemin pour les matériaux holographiques

        // Créer les dossiers des matériaux s'ils n'existent pas
        if (!Directory.Exists(materialsPath))
        {
            Directory.CreateDirectory(materialsPath);
        }

        if (!Directory.Exists(holoMaterialsPath))
        {
            Directory.CreateDirectory(holoMaterialsPath);
        }

        // Obtenir tous les fichiers de texture .png
        string[] textureFiles = Directory.GetFiles(texturesPath, "*.png");
        Shader customShader = Shader.Find("Universal Render Pipeline/Lit"); // Cherche le shader URP Lit
        Shader holoShader = Shader.Find("Custom/HologramShader"); // Chemin vers le shader holographique

        // Vérifie si les shaders existent
        if (customShader == null)
        {
            Debug.LogError("Shader 'Universal Render Pipeline/Lit' not found. Please ensure URP is installed.");
            return;
        }

        if (holoShader == null)
        {
            Debug.LogError("Shader 'Custom/HologramShader' not found. Please ensure the custom shader is available.");
            return;
        }

        // Parcours chaque fichier de texture
        foreach (string textureFile in textureFiles)
        {
            string textureName = Path.GetFileNameWithoutExtension(textureFile); // Obtenir le nom de la texture sans extension
            Material material = new Material(customShader); // Créer un nouveau matériau avec le shader URP Lit

            // Charger la texture
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(textureFile);
            if (texture != null)
            {
                // Assigner la texture au Base Map du matériau
                material.SetTexture("_BaseMap", texture); // Utiliser _BaseMap au lieu de _BaseColor
                material.SetColor("_BaseColor", Color.white); // Assure que la couleur de base est blanche
                material.SetFloat("_Metallic", 0.0f); // Définit le niveau de métal à 0
                material.SetFloat("_Smoothness", 0.5f); // Définit la rugosité à un niveau moyen

                Debug.Log($"Loaded Texture: {texture.name}, Width: {texture.width}, Height: {texture.height}");
            }
            else
            {
                Debug.LogWarning($"Texture not found or could not be loaded: {textureFile}");
                continue;
            }

            // Créer le chemin du fichier pour le matériau
            string materialPath = Path.Combine(materialsPath, textureName + ".mat");
            AssetDatabase.CreateAsset(material, materialPath); // Créer le matériau dans l'Asset Database
            Debug.Log($"Material created: {materialPath}");

            // Créer une copie du matériau avec le shader holographique
            Material holoMaterial = new Material(holoShader);
            holoMaterial.SetTexture("_MainTex", texture); // Utiliser _MainTex pour le shader holographique
            holoMaterial.SetColor("_Color", new Color(1, 1, 1, 0.25f)); // Définir la couleur et la transparence

            // Créer le chemin du fichier pour le matériau holographique
            string holoMaterialPath = Path.Combine(holoMaterialsPath, textureName + "_Holo.mat");
            AssetDatabase.CreateAsset(holoMaterial, holoMaterialPath); // Créer le matériau holographique dans l'Asset Database
            Debug.Log($"Holo Material created: {holoMaterialPath}");
        }

        // Sauvegarder les changements
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Materials created and converted to URP successfully!");
    }
}

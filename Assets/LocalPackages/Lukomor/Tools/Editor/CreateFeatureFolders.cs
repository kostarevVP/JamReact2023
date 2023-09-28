using UnityEditor;
using System.IO;

public class CreateFolders : AssetPostprocessor
{
    private static readonly string[] featureFolders = new string[] { "api", "Implementation", "Installers", "Prefabs", "Representation", "Resources" };
    private static readonly string[] serviceFolders = new string[] { "api", "Implementation", "Installers", "Prefabs", "Representation", "Resources" };

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string assetPath in importedAssets)
        {
            if (assetPath.EndsWith("Feature") && assetPath.Contains("/Features/"))
            {
                CreateFoldersAtPath(assetPath, featureFolders);
            }
            else if (assetPath.EndsWith("Service") && assetPath.Contains("/Services/"))
            {
                CreateFoldersAtPath(assetPath, serviceFolders);
            }
            else if (assetPath.EndsWith("Feature") && assetPath.Contains("/GameFeatures/"))
            {
                CreateFoldersAtPath(assetPath, serviceFolders);
            }
        }
    }

    private static void CreateFoldersAtPath(string path, string[] folders)
    {
        string parentFolderName = Path.GetFileName(Path.GetDirectoryName(path));
        foreach (string folderName in folders)
        {
            string folderPath = Path.Combine(path, folderName);
            if (folderName.EndsWith("/UI"))
            {
                folderPath = Path.Combine(folderPath, "UI");
            }

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(path, folderName);
            }
        }
    }
}


using System.IO;
using UnityEditor;

public class CustomCreateEntries : Editor
{
    [MenuItem("Assets/Create/Dialogue", priority = 0)]
    public static void CreateEmptyYarnFile()
    {
        string filePath;

        if (Selection.activeObject != null)
        {
            filePath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (Directory.Exists(filePath))
                filePath = Path.Combine(filePath, "NewDialogue.yarn");
            else if (File.Exists(filePath))
                filePath = Path.Combine(Path.GetDirectoryName(filePath), "NewDialogue.yarn");
        }
        else
            filePath = "Assets/NewDialogue.yarn";

        filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);

        var file = new StreamWriter(filePath);
        file.WriteLine("title: Start");
        file.WriteLine("tags: ");
        file.WriteLine("---");
        file.WriteLine("===");
        file.Close();

        AssetDatabase.ImportAsset(filePath);
    }
}

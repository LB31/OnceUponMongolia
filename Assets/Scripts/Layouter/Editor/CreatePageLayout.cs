using System.IO;
using UnityEditor;

namespace Layouter
{
    public class CreatePageLayout : Editor
    {
        private const string FileName = "PageLayout.asset";
        
        [MenuItem("Assets/Create/Page Layout", priority = 0)]
        public static void CreateEmptyPageLayout()
        {
            string filePath;

            if (Selection.activeObject != null)
            {
                filePath = AssetDatabase.GetAssetPath(Selection.activeObject);

                if (Directory.Exists(filePath))
                    filePath = Path.Combine(filePath, FileName);
                else if (File.Exists(filePath))
                    filePath = Path.Combine(Path.GetDirectoryName(filePath) ?? "Assets", FileName);
            }
            else
                filePath = "Assets/" + FileName;

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
}
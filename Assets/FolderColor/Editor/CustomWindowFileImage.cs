using UnityEditor;
using UnityEngine;

namespace FolderColor
{
    public class CustomWindowFileImage : EditorWindow
    {
        string assetPath;
        Vector2 scrollPosition; // Scroll pozisyonu için değişken

        public static void ShowWindow(string assetPathGive)
        {
            CustomWindowFileImage window = GetWindow<CustomWindowFileImage>("Custom Folder");
            window.assetPath = assetPathGive;
            window.Show();
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 100, 50), "None"))
            {
                if (ProjectAssetViewerCustomisation.modificationData.assetModified.Contains(assetPath))
                {
                    RemoveReference(assetPath);
                    ProjectAssetViewerCustomisation.SaveData();
                }

                Close();
            }

            string[] texturesPath = AssetDatabase.FindAssets("t:texture2D", new[] { "Assets/FolderColor" });

            int buttonsPerRow = 4;
            float buttonPadding = 10f;
            float buttonHeight = 100f;
            float buttonWidth = (position.width - (buttonsPerRow + 1) * buttonPadding) / buttonsPerRow;

            // Toplam yükseklik hesapla (ikonların kaç satır tuttuğuna bağlı olarak)
            int totalRows = Mathf.CeilToInt((float)texturesPath.Length / buttonsPerRow);
            float totalHeight = totalRows * (buttonHeight + buttonPadding) + buttonPadding;

            // Scroll View başlangıcı
            scrollPosition = GUI.BeginScrollView(
                new Rect(0, 60, position.width, position.height - 60), // Kaydırılabilir alan
                scrollPosition,
                new Rect(0, 0, position.width - 20, totalHeight)      // İçerik boyutu
            );

            for (int i = 0; i < texturesPath.Length; i++)
            {
                Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(texturesPath[i]), typeof(Texture2D));

                float x = (i % buttonsPerRow) * (buttonWidth + buttonPadding) + buttonPadding;
                float y = Mathf.Floor(i / buttonsPerRow) * (buttonHeight + buttonPadding) + buttonPadding;

                if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), texture))
                {
                    if (ProjectAssetViewerCustomisation.modificationData.assetModified.Contains(assetPath)) 
                        RemoveReference(assetPath);

                    ProjectAssetViewerCustomisation.modificationData.assetModified.Add(assetPath);
                    ProjectAssetViewerCustomisation.modificationData.assetModifiedTexturePath.Add(AssetDatabase.GUIDToAssetPath(texturesPath[i]));
                    ProjectAssetViewerCustomisation.SaveData();

                    Close();
                }
            }

            GUI.EndScrollView(); // Scroll View sonu
        }

        private static void RemoveReference(string assetPath)
        {
            int i = ProjectAssetViewerCustomisation.modificationData.assetModified.IndexOf(assetPath);
            ProjectAssetViewerCustomisation.modificationData.assetModified.RemoveAt(i);
            ProjectAssetViewerCustomisation.modificationData.assetModifiedTexturePath.RemoveAt(i);
        }
    }
}

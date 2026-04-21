using UnityEngine;
using UnityEditor;

public class MaterialFixer : EditorWindow
{
    [MenuItem("Kodlama Destegi/Materyalleri URP Yap")]
    public static void FixMaterials()
    {
        // Projedeki tüm materyalleri tara
        string[] guids = AssetDatabase.FindAssets("t:Material");
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null)
            {
                string sName = mat.shader.name;

                // Eðer shader ismi RCCP içeriyorsa veya hatalýysa (pembe ise)
                if (sName.Contains("RCCP") || sName.Contains("InternalError") || sName.Contains("Standard"))
                {
                    // Materyali URP Lit shader'ýna zorla
                    mat.shader = Shader.Find("Universal Render Pipeline/Lit");

                    // Deðiþiklikleri kaydet
                    EditorUtility.SetDirty(mat);
                    count++;
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("RCCP Destegi: " + count + " adet materyal URP Lit olarak güncellendi!");
    }
}
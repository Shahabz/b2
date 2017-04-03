using System.Collections;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

//this utility class was downloaded from http://www.jacobpennock.com/Blog/?page_id=715
//it allows user to create custom assets that can be created from the Unity editor
//any changes you make to them will remain persistent throughout edit time and runtime
//making them perfect for storing dialogue data for an entire game.

public static class CustomAssetUtility
{
    /// <summary>
    /// Creates the asset at Selection.activeObject.
    /// </summary>
    /// <param name="focusAsset">If set to <c>true</c> focus asset.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static void CreateAsset<T> (bool focusAsset) where T : ScriptableObject
	{
	    string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "")
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");

        CreateAssetAtPath<T>(assetPathAndName, focusAsset);
	}

    public static void CreateAssetAtPath<T> (string path, bool focusAsset) where T : ScriptableObject
    {
        if (!path.EndsWith(".asset"))
        {
            path += ".asset";
        }

        T asset = ScriptableObject.CreateInstance<T> ();

        AssetDatabase.CreateAsset (asset, path);

        AssetDatabase.SaveAssets ();
        if (focusAsset)
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
#endif

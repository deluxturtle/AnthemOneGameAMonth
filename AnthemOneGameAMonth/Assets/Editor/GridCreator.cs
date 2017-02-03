using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Author: Andrew Seba
/// Description: Opens the grid settings window and checks to see if it needs to create the assets.
/// </summary>
public class GridCreator : Editor {

    //first time ran sense editor has been opened.
    //static bool hasRun = false;

    [MenuItem("GridMaster/GridSettings")]
	static void OpenGridSettings()
    {
        EditorWindow.GetWindow(typeof(GridCreatorEditorWindow));
        //if(hasRun == false)
        //{
        //    CreateAssets();
        //}
    }

    //static void CreateAssets()
    //{
    //    TextAsset data = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/GridMasterSaveData.txt", typeof(TextAsset));
    //    if (data == null)
    //    {
    //        string dataString = "Last run @";
    //        System.IO.File.WriteAllText(Application.dataPath + "/gridMasterData.txt",
    //            dataString);
    //        AssetDatabase.Refresh();
    //        Debug.Log("")
    //    }

    //    hasRun = true;
    //}
}

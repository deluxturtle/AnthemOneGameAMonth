using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Author: Andrew Seba
/// Description: Grid settings window. for the unity editor.
/// </summary>
public class GridCreatorEditorWindow : EditorWindow {

    private bool gridExsits = false;
    private GameGrid gameGrid;
    private Material gameTileMat;
    private bool overlay;//If the user wants to have the grid overlay on a world terrain or object.
    private int row = 5;
    private int col = 5;
    private int maxRows = 100;
    private int maxCol = 100;
    private float tileSize = 1f;

    private float overlayRayBegin = 100f;
    private float overlayRayDistance = 500f;

    private static GameGridTile[,] tempGameGridTiles;
    
    private string errorMessege;


    //Whenever the window needs to refresh it will run this.
    void OnGUI()
    {

        if(gameTileMat == null)
        {
            //gameTileMat = (Material)Resources.Load("Basic");
            if(gameTileMat == null)
            {
                gameTileMat = new Material(Shader.Find("Diffuse"));
            }

        }

        //Check to see if the grid is already in the scene.
        if (GameObject.FindObjectOfType<GameGrid>())
        {
            gameGrid = GameObject.FindObjectOfType<GameGrid>();
            gridExsits = true;
        }
        else
        {
            gridExsits = false;
        }

        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
        if (gridExsits == false)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rows", GUILayout.Width(100));
            row = EditorGUILayout.IntField(row, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Collumns", GUILayout.Width(100));
            col = EditorGUILayout.IntField(col, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            //Limit values
            if (row > maxRows)
            {
                row = maxRows;
            }
            if (col > maxCol)
            {
                col = maxCol;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Overlay", "Overlays to the height of object or terrain From Pivot Center of object", GUILayout.Width(100));
            overlay = EditorGUILayout.Toggle(overlay);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create Grid"))
            {
                gameGrid = new GameObject("gameGrid").AddComponent<GameGrid>();
                gameGrid.gridTiles = new List<GameGridTile>();

                //Create the tile for the grid
                GameObject tileObj = new GameObject();
                tileObj.AddComponent<MeshFilter>();
                tileObj.GetComponent<MeshFilter>().mesh = CreatePlane(tileSize, tileSize);
                tileObj.transform.Rotate(new Vector3(-90,0,0));
                tileObj.AddComponent<MeshRenderer>();
                tileObj.GetComponent<MeshRenderer>().material = gameTileMat; //AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");
                tileObj.AddComponent<MeshCollider>();
                tileObj.GetComponent<MeshCollider>();

                tempGameGridTiles = new GameGridTile[row, col];

                if (overlay)
                {
                    GameObject selectedObj;
                    if ((selectedObj = Selection.activeTransform.gameObject) != null)
                    {

                        //Assign mask to the world objects so we don't have to raycastall
                        int worldLayer = 8;
                        int worldMask = 1 << worldLayer;
                        foreach(Transform child in selectedObj.transform)
                        {
                            child.gameObject.layer = worldLayer;
                        }

                        // get center of the object
                        //spiral out from the center

                        Debug.Log("Starting to Overlay On: " + Selection.activeTransform.gameObject.name);
                        Vector3 center = selectedObj.transform.position;
                        Vector3 rayBegin = center + new Vector3(0, overlayRayBegin, 0);
                        Ray ray = new Ray(rayBegin, Vector3.down); //Raycast down on the object we are overlaying on.
                        RaycastHit hitInfo;
                        Physics.Raycast(ray, out hitInfo, overlayRayDistance, worldMask, QueryTriggerInteraction.Ignore);

                        Debug.Log(hitInfo.collider.gameObject.name);


                        //center object
                        //Not going to skip empty tiles for now!
                        //If hit
                            //match the height of the tile to the hit point
                        //else
                            //put the tile at the height of the world object we are overlaying on.
                            
                        GameObject tempTile = Instantiate(tileObj);
                        tempTile.transform.position = new Vector3();
                    }
                }
                else
                {
                    for (int i = 0; i < col; i++)
                    {
                        for (int j = 0; j < row; j++)
                        {
                            GameObject tempTile = Instantiate(tileObj);
                            tempTile.transform.position = new Vector3(gameGrid.transform.position.x + i * (tempTile.transform.localScale.x + tileSize),
                                gameGrid.transform.position.y,
                                gameGrid.transform.position.z + j * (tempTile.transform.localScale.z + tileSize));
                            tempTile.transform.parent = gameGrid.transform;
                            tempTile.name = i + "," + j;
                            tempTile.AddComponent<GameGridTile>();
                            tempGameGridTiles[i, j] = tempTile.GetComponent<GameGridTile>();
                        }
                    }
                }
                DestroyImmediate(tileObj);

                //Make connections.
                MakeConnections();

                gridExsits = true;
            }
        }
        else //After Grid is in scene this is what will show.
        {
            if (GUILayout.Button("Clear Grid (Reset)"))
            {
                DestroyImmediate(FindObjectOfType<GameGrid>().gameObject);
            }
        }
    }

    private void MakeConnections()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (i > 0 && tempGameGridTiles[i - 1, j] != null)
                {
                    tempGameGridTiles[i, j].West = tempGameGridTiles[i - 1, j];
                }
                if (i < col - 1 && tempGameGridTiles[i + 1, j] != null)
                {
                    tempGameGridTiles[i, j].East = tempGameGridTiles[i + 1, j];
                }
                if (j > 0 && tempGameGridTiles[i, j - 1] != null)
                {
                    tempGameGridTiles[i, j].South = tempGameGridTiles[i, j - 1];
                }
                if (j < row - 1 && tempGameGridTiles[i, j + 1] != null)
                {
                    tempGameGridTiles[i, j].North = tempGameGridTiles[i, j + 1];
                }
            }
        }
    }

    Mesh CreatePlane(float width, float height)
    {
        Mesh mesh = new Mesh();
        mesh.name = "SimplePlane";
        mesh.vertices = new Vector3[]
        {
            new Vector3(-width, -height, 0.01f),
            new Vector3(width, -height, 0.01f),
            new Vector3(width, height, 0.01f),
            new Vector3(-width, height, 0.01f)
        };
        mesh.uv = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0)
        };

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        mesh.RecalculateNormals();
        return mesh;
    }

    void OnDrawGizmos()
    {
        if(gridExsits == false)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Vector3.zero, Vector3.right);
        }
    }
}

using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Author: Andrew Seba
/// Description: Loads map from XML
/// </summary>
public class LoadFromXML : MonoBehaviour {

    public TextAsset mapInformation;
    public int layerWidth;
    public int layerHeight;

    GameObject collisionParent;
    private Sprite[] spriteTiles;
    //Entities
    private const int VILLAGER_M = 90;
    private const int VILLAGER_BLU_M = 245;
    private const int VILLAGER_RED_M = 252;
    private const int KNIGHT = 114;
    //Terrain
    private const int FOREST = 6;
    
    void Awake()
    {
        collisionParent = new GameObject("CollisionGridLayer");
        StartCoroutine("LoadMap");
    }

    IEnumerator LoadMap()
    {
        //If we load it during runtime we need to clear the level to load in first.
        //Clear Level();
        //yield return new WaitForEndOfFrame();

        try
        {
            spriteTiles = Resources.LoadAll<Sprite>("Tile-set(Toen's)");
        }
        catch
        {
            Debug.LogWarning("Couldn't load in sprite sheet.");
        }

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(mapInformation.text);

        //I think layer is a "Tiled" element in the xml
        XmlNodeList layerNames = xmlDoc.GetElementsByTagName("layer");

        XmlNode tilesetInfo = xmlDoc.SelectSingleNode("map").SelectSingleNode("tileset");
        float tileWidth = (float.Parse(tilesetInfo.Attributes["tilewidth"].Value) / (float)16);
        float tileHeight = (float.Parse(tilesetInfo.Attributes["tileheight"].Value) / (float)16);

        //Generate Collision grid for mouse input.
        int width = int.Parse(xmlDoc.SelectSingleNode("map").Attributes["width"].Value);
        int height = int.Parse(xmlDoc.SelectSingleNode("map").Attributes["height"].Value);

        Tile[,] allTiles = new Tile[width, height];


        
        for (int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                GameObject tempSprite = new GameObject("gid(" + i + "," + j + ")");
                Tile tempTile = tempSprite.AddComponent<Tile>();
                tempTile.x = i;
                tempTile.y = j;
                allTiles[i, j] = tempTile;
                tempSprite.AddComponent<BoxCollider2D>();
                //set position
                tempSprite.transform.position = new Vector3((tileWidth * i), (tileHeight * j));
                tempSprite.tag = "Tile";
                tempSprite.transform.parent = collisionParent.transform;
            }
        }

        //Build Basic Connections
        foreach(Tile tile in allTiles)
        {
            //Left
            if (tile.x - 1 >= 0)
            {
                tile.Connections.Add(new ScriptConnection(tile.gameObject, allTiles[tile.x - 1, tile.y].gameObject, 0));
            }
            if(tile.x + 1 < width)
            {
                tile.Connections.Add(new ScriptConnection(tile.gameObject, allTiles[tile.x + 1, tile.y].gameObject, 0));
            }
            if(tile.y - 1 >= 0)
            {
                tile.Connections.Add(new ScriptConnection(tile.gameObject, allTiles[tile.x, tile.y - 1].gameObject, 0));
            }
            if(tile.y + 1 < height)
            {
                tile.Connections.Add(new ScriptConnection(tile.gameObject, allTiles[tile.x, tile.y + 1].gameObject, 0));
            }
        }



        //for each layer that exists
        foreach (XmlNode layerInfo in layerNames)
        {
            layerWidth = int.Parse(layerInfo.Attributes["width"].Value);
            layerHeight = int.Parse(layerInfo.Attributes["height"].Value);

            //Pull out of the data node
            XmlNode tempNode = layerInfo.SelectSingleNode("data");

            int verticalIndex = layerHeight - 1;
            int horizontalIndex = 0;

            foreach(XmlNode tile in tempNode.SelectNodes("tile"))
            {
                int spriteValue = int.Parse(tile.Attributes["gid"].Value);


                //if not empty
                if(spriteValue > 0)
                {
                    Sprite[] currentSpriteSheet = spriteTiles;

                    //Create a sprite
                    GameObject tempSprite = new GameObject(layerInfo.Attributes["name"].Value + " <" + horizontalIndex + ", " + verticalIndex + ">");
                    

                    //Make a sprite renderer.
                    SpriteRenderer spriteRend = tempSprite.AddComponent<SpriteRenderer>();
                    //get sprite from sheet.
                    spriteRend.sprite = currentSpriteSheet[spriteValue - 1];
                    //set position
                    tempSprite.transform.position = new Vector3((tileWidth * horizontalIndex), (tileHeight * verticalIndex));
                    //set sorting layer
                    spriteRend.sortingLayerName = layerInfo.Attributes["name"].Value;

                    //set parent
                    GameObject parent = GameObject.Find(layerInfo.Attributes["name"].Value + "Layer");
                    if (parent == null)
                    {
                        parent = new GameObject();
                        parent.name = layerInfo.Attributes["name"].Value + "Layer";
                    }
                    tempSprite.transform.parent = GameObject.Find(layerInfo.Attributes["name"].Value + "Layer").transform;
                    //tempSprite.tag = "Tile";
                    

                    if (layerInfo.Attributes["name"].Value == "Background")
                    {
                        
                    }
                    else if (layerInfo.Attributes["name"].Value == "Entities")
                    {
                        tempSprite.tag = "Entity";
                        
                        int value = spriteValue - 1;
                        switch (value)
                        {
                            case VILLAGER_BLU_M:
                            case VILLAGER_M:
                                tempSprite.name = "Villager";
                                Human tempVillager = tempSprite.AddComponent<Human>();
                                tempVillager.SetupHuman(Class.Villager, Faction.Blue, horizontalIndex, verticalIndex);
                                FindParent(tempVillager);
                                if(value == VILLAGER_BLU_M)
                                {
                                    tempVillager.Faction = Faction.Blue;
                                }
                                else if(value == VILLAGER_RED_M)
                                {
                                    tempVillager.Faction = Faction.Red;
                                }
                                break;
                            case KNIGHT:
                                tempSprite.name = "Knight";
                                Human knight = tempSprite.AddComponent<Human>();
                                knight.x = horizontalIndex;
                                knight.y = verticalIndex;
                                knight.ClassType = Class.Knight;
                                FindParent(knight);
                                break;
                            default:
                                Debug.Log("Unknown Entity placed in map(no info loaded for sprite index: " + (spriteValue - 1) + ").");
                                break;
                        }

                        
                    }
                    else if(layerInfo.Attributes["name"].Value == "Terrain")
                    {
                        switch(spriteValue - 1)
                        {
                            case FOREST:
                                tempSprite.name = "Forest";
                                Terrain tempForest = tempSprite.AddComponent<Terrain>();
                                tempForest.x = horizontalIndex;
                                tempForest.y = verticalIndex;
                                tempForest.terrainType = TerrainType.Forest;
                                FindParent(tempForest);
                                break;
                            default:
                                break;
                        }
                    }
                    


                }
                horizontalIndex++;
                if(horizontalIndex % layerWidth == 0)
                {
                    //Increase our vertical location
                    verticalIndex--;
                    //reset our horizontal location
                    horizontalIndex = 0;
                }
            }

        }//End of placing sprites
        //rebuild costs in the connections

        yield break;
    }

    void FindParent(Tile tile)
    {
        foreach (Transform tilObj in collisionParent.transform)
        {
            if (tilObj.GetComponent<Tile>().x == tile.x && tilObj.GetComponent<Tile>().y == tile.y)
            {
                Tile tileScript = tilObj.GetComponent<Tile>();
                if (tile is Human)
                {
                    Human tempHuman = (Human)tile;
                    tempHuman.tileOccuping = tileScript;
                    tileScript.isOccupied = true;
                }
                else if (tile is Terrain)
                {
                    Terrain tempTerrain = (Terrain)tile;
                    int terrainCost = 0;
                    switch (tempTerrain.terrainType)
                    {
                        case TerrainType.Forest:
                            terrainCost = 1;
                            break;
                        default:
                            terrainCost = 0;
                            break;
                    }
                    foreach(ScriptConnection conn in tileScript.Connections)
                    {
                        Tile surroundingForest = conn.goingTo.GetComponent<Tile>();
                        
                        foreach(ScriptConnection goingToForest in surroundingForest.Connections)
                        {
                            if(goingToForest.goingTo == tileScript.gameObject)
                            {
                                goingToForest.cost = terrainCost;
                                break;
                            }
                        }
                    }
                    
                }
            }
        }
    }
}

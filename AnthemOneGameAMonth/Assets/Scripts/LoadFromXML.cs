using UnityEngine;
using System.Xml;
using System.Collections;

/// <summary>
/// Author: Andrew Seba
/// Description: Loads map from XML
/// </summary>
public class LoadFromXML : MonoBehaviour {

    public TextAsset mapInformation;
    public int layerWidth;
    public int layerHeight;

    private Sprite[] spriteTiles;
    
    void Awake()
    {
        StartCoroutine("LoadMap");
    }

    IEnumerator LoadMap()
    {
        //If we load it during runtime we need to clear the level to load in first.
        //Clear Level();
        //yield return new WaitForEndOfFrame();

        try
        {
            spriteTiles = Resources.LoadAll<Sprite>("Tile-set - Toen's Medieval Strategy (16x16) - v.1.0");
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

        //for each layer that exists
        foreach(XmlNode layerInfo in layerNames)
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

                    //add the tile script to it
                    Tile tempTile = tempSprite.AddComponent<Tile>();
                    tempTile.x = horizontalIndex;
                    tempTile.y = verticalIndex;

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
                    tempSprite.tag = "Tile";

                    if(layerInfo.Attributes["name"].Value == "Background")
                    {
                        tempSprite.AddComponent<BoxCollider2D>();
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

        }
        yield break;
    }
}

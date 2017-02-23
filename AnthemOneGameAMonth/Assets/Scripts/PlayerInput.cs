using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Author: Andrew Seba
/// Description: Controls mouse selection and stuff of that sort.
/// </summary>
public class PlayerInput : MonoBehaviour {

    [Tooltip("The cursor object you'd like to use.")]
    public GameObject cursor;
    [Tooltip("Square Cursor for seeing your selection on the grid.")]
    public GameObject tileCursor;
    public GameObject highlightGraphic;
    public AudioClip clickSFX;


    private Plane plane = new Plane();
    private GameObject selectedEnt = null;
    private GameObject previousTile = null;
    private GameObject selectedTile;
    private bool hitTile = false;
    private List<Tile> allTiles = new List<Tile>();
    private List<GameObject> moveableTiles = new List<GameObject>();
    private List<GameObject> tileHighlights = new List<GameObject>(); //for deleting all the graphics later.
    private AudioSource audioSrc;
    private bool leftMouseClick = false;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        if(audioSrc == null)
        {
            audioSrc = gameObject.AddComponent<AudioSource>();
        }
        audioSrc.clip = clickSFX;

        Cursor.visible = false;
        plane.SetNormalAndPosition(Vector3.forward, Vector3.up);
        foreach(Tile tile in GameObject.FindObjectsOfType<Tile>())
        {
            allTiles.Add(tile);
        }
        StartCoroutine("SelectUnit");
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitColliders;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        hitColliders = Physics2D.OverlapPointAll(mousePos);
        

        hitTile = false;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.GetComponent<Tile>())
            {
                selectedTile = hitColliders[i].gameObject;
                //selectedTile.GetComponent<Renderer>().material.color = Color.yellow;


                if (selectedTile != previousTile)
                {
                    UpdateSelectedTile();
                }

                hitTile = true;
            }
        }
        //get closest tile
        if (!hitTile)
        {            
            foreach(GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
            {
                //get closest one
                if(previousTile != null)
                {
                    if (Vector3.Distance(tile.transform.position, mousePos) < Vector2.Distance(previousTile.transform.position, mousePos))
                    {
                        if (selectedTile != previousTile)
                        {
                            UpdateSelectedTile();
                        }
                    }
                }
            }
        }

        cursor.transform.position = mousePos;

        //Get input
        if (Input.GetButtonDown("Fire1"))
        {
            audioSrc.Play();
        }
    }

    IEnumerator SelectUnit()
    {
        bool selectedUnit = false;
        while (!selectedUnit)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                foreach (GameObject entity in GameObject.FindGameObjectsWithTag("Entity"))
                {
                    Tile tileScript = entity.GetComponent<Tile>();
                    Tile selectedTileScript = selectedTile.GetComponent<Tile>();
                    if(tileScript is Human)
                    {
                        Human tempHuman = (Human)tileScript;
                        if(tempHuman.tileOccuping == selectedTileScript)
                        {
                            selectedEnt = tempHuman.gameObject;
                            selectedUnit = true;
                            yield return new WaitForFixedUpdate();
                            break;
                        }
                    }
                }
                
            }
            yield return null;
        }
        StartCoroutine("SelectDestination");
        StopCoroutine("SelectUnit");
    }

    /// <summary>
    /// Lets the player select the tile he wants to move the unit to with pathfinding.
    /// </summary>
    IEnumerator SelectDestination()
    {
        moveableTiles = new List<GameObject>();
        HighlightMoveableTiles(selectedEnt);
        while (true)
        {
            //If nothing is null.
            if (selectedTile != null && selectedEnt != null && Input.GetButtonDown("Fire1") && selectedTile.GetComponent<Tile>().occupiedBy == null)
            {
                Tile sTile = selectedTile.GetComponent<Tile>();
                //If so Move there!!
                if (moveableTiles.Contains(moveableTiles.Find(tile => (tile.GetComponent<Tile>().x == sTile.x && tile.GetComponent<Tile>().y == sTile.y))))
                {
                    selectedEnt.GetComponent<Human>().MoveTo(selectedTile);
                    yield return new WaitForFixedUpdate();
                    break;
                }
                //break;
            }
            yield return null;
        }

        //clear highlihgt?
        foreach (GameObject highlight in tileHighlights)
        {
            Destroy(highlight);
        }
        moveableTiles.Clear();
        StartCoroutine("SelectUnit");

    }

    /// <summary>
    /// When the selected tile changes this function will run.
    /// </summary>
    void UpdateSelectedTile()
    {
        previousTile = selectedTile;
        tileCursor.transform.position = previousTile.transform.position;
    }

    void HighlightMoveableTiles(GameObject selectedUnit)
    {

        //Clear tile range values
        foreach(GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            tile.GetComponent<Tile>().range = 0;
        }

        Human humanInfo = selectedEnt.GetComponent<Human>();
        int speed = humanInfo.Speed;

        //Full fill 
        //lets get the first surrounding tiles
        AddNeighbors(1, humanInfo.tileOccuping, moveableTiles, false);

        for(int range = 1; range <= speed; range++)
        {
            foreach(GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
            {
                if(tile.GetComponent<Tile>().range == range)
                {
                    if (range < speed)
                    {
                        AddNeighbors(range + 1, tile.GetComponent<Tile>(), moveableTiles, false);
                    }
                }
            }
        }


        //add highlight graphic to scene
        foreach(GameObject tile in moveableTiles)
        {
            GameObject tileHighlight = (GameObject)Instantiate(highlightGraphic, tile.transform.position, Quaternion.identity);
            tileHighlights.Add(tileHighlight);

        }
        

    }

    void AddNeighbors(int pRange, Tile tileInfo, List<GameObject> pMoTiles, bool highlightOccupied)
    {
        foreach(ScriptConnection connection in tileInfo.Connections)
        {
            Tile goingTo = connection.goingTo.GetComponent<Tile>();
            
            if (!pMoTiles.Contains(goingTo.gameObject))
            {
                if(goingTo.occupiedBy == null || (goingTo.occupiedBy != null && goingTo.occupiedBy.Faction == selectedEnt.GetComponent<Human>().Faction))
                {
                    goingTo.range += pRange + connection.cost;
                    pMoTiles.Add(goingTo.gameObject);
                }
            }
        }
    }
}
using UnityEngine;
using System.Collections;

/// <summary>
/// Author: Andrew Seba
/// Description: Controls mouse selection and stuff of that sort.
/// </summary>
public class PlayerInput : MonoBehaviour {

    [Tooltip("The cursor object you'd like to use.")]
    public GameObject cursor;

    private Plane plane = new Plane();
    private GameObject selectedEnt = null;
    private GameObject previousTile = null;
    private GameObject selectedTile;
    private bool hitTile = false;

    void Start()
    {
        plane.SetNormalAndPosition(Vector3.forward, Vector3.up);
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
                        if (tile != previousTile)
                        {
                            selectedTile = tile;
                            //selectedTile.GetComponent<Renderer>().material.color = Color.yellow;
                        }
                        if (selectedTile != previousTile)
                        {
                            UpdateSelectedTile();
                        }
                    }
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            foreach(GameObject entitie in GameObject.FindGameObjectsWithTag("Entity"))
            {
                Tile tileScript = entitie.GetComponent<Tile>();
                Tile selectedTileScript = selectedTile.GetComponent<Tile>();
                if(tileScript != null)
                {
                    if(tileScript.x == selectedTileScript.x && tileScript.y == selectedTileScript.y)
                    {

                    }
                }
                
            }
        }
    }

    /// <summary>
    /// When the selected tile changes this function will run.
    /// </summary>
    void UpdateSelectedTile()
    {
        //Change stuff on the previous.
        if (previousTile != null)
        {
            //previousTile.GetComponent<Renderer>().material.color = Color.black;
        }
        previousTile = selectedTile;
        cursor.transform.position = previousTile.transform.position;

        //Debug.Log(previousTile.name);
    }
}
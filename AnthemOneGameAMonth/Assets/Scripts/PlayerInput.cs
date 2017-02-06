using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    //Ground Zero
    Plane plane = new Plane();
    GameObject previousTile = null;
    GameObject selectedTile;
    bool hitTile = false;

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
                selectedTile.GetComponent<Renderer>().material.color = Color.yellow;


                if (selectedTile != previousTile)
                {
                    if (previousTile != null)
                        previousTile.GetComponent<Renderer>().material.color = Color.black;
                    previousTile = selectedTile;
                }

                hitTile = true;
            }
        }
        //get closest tile
        if (!hitTile)
        {
            float distance;
            
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
                            selectedTile.GetComponent<Renderer>().material.color = Color.yellow;
                        }
                        if (selectedTile != previousTile)
                        {
                            previousTile.GetComponent<Renderer>().material.color = Color.black;
                            previousTile = selectedTile;
                        }
                    }
                }
            }
            //if (plane.Raycast(ray, out distance))
            //{
            //    Vector3 point = ray.GetPoint(distance);
            //    //Get tiles closest to point
            //    foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
            //    {

            //        //Get closest one
            //        if (previousTile != null)
            //        {
            //            if (Vector3.Distance(tile.transform.position, point) < Vector3.Distance(previousTile.transform.position, point))
            //            {
            //                if (tile != previousTile)
            //                {
            //                    selectedTile = tile;
            //                    selectedTile.GetComponent<Renderer>().material.color = Color.yellow;
            //                }
            //                if (selectedTile != previousTile)
            //                {
            //                    previousTile.GetComponent<Renderer>().material.color = Color.black;
            //                    previousTile = selectedTile;
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}
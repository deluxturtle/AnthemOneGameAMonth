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
        plane.SetNormalAndPosition(Vector3.up, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, 500f);



        hitTile = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider.gameObject.GetComponent<GameGridTile>())
            {
                selectedTile = hits[i].collider.gameObject;
                selectedTile.GetComponent<Renderer>().enabled = true;
                

                if (selectedTile != previousTile)
                {
                    if (previousTile != null)
                        previousTile.GetComponent<Renderer>().enabled = false;
                    previousTile = selectedTile;
                }

                hitTile = true;
            }
        }
        //get closest tile
        if (!hitTile)
        {
            float distance;
            if(plane.Raycast(ray, out distance))
            {
                Vector3 point = ray.GetPoint(distance);
                //Get tiles closest to point
                foreach(GameObject tile in FindObjectOfType<GameGridTile>().gameObject)
                {

                    //Get closest one

                    if(Vector3.Distance(tile.transform.position, point) < Vector3.Distance(previousTile.transform.position, point))
                    {
                        if(tile != previousTile)
                        selectedTile = tile;
                        if (selectedTile != previousTile)
                        {
                            if (previousTile != null)
                                previousTile.GetComponent<Renderer>().enabled = false;
                            previousTile = selectedTile;
                        }
                    }
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Any Game Object that Tiles can Have on them, including doors, keys, walls, etc.
    public List<GameObject> containedObjects = new List<GameObject>();
    

    //Places GameObject on this tile
    public void addObject(GameObject go)
    {
        go.transform.SetParent(transform, false);
        containedObjects.Add( go );
    }

    public void removeObject(GameObject go)
    {
        if (containedObjects.Contains(go))
        {
            containedObjects.Remove(go);
            Destroy(go);
        }
    }

    public void clearObjects()
    {
        foreach (GameObject go in containedObjects)
        {
            Destroy(go);
        }
    }

    public void moveObjectTo(GameObject go, Tile tile) {
        if (containedObjects.Contains(go))
        {
            containedObjects.Remove(go);
            tile.containedObjects.Add(go);
        }
    }

    //Check what (if any) Game Object is on this tile
    public List<GameObject> getContainedObjects()
    {
        return containedObjects;
    }

    public List<Interactable> containsInteractable()
    {
        List<Interactable> interactables = new List<Interactable>();
        foreach(GameObject go in containedObjects)
        {
            if(go.GetComponent<Interactable>() != null)
            {
                interactables.Add(go.GetComponent<Interactable>());
            }
        }
        return interactables;
    }

}

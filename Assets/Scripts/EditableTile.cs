using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EditableTile : Tile
{
    private const int TOTAL_TYPES = 11;
    private const int EMPTY = 0;
    private const int WALL = 1;
    private const int PLAYER_START = 2;
    private const int PLAYER_END = 3;
    private const int KEY_LINK_0 = 4;
    private const int DOOR_LINK_0 = 5;
    private const int KEY_LINK_1 = 6;
    private const int DOOR_LINK_1 = 7;
    private const int KEY_LINK_2 = 8;
    private const int DOOR_LINK_2 = 9;
    private const int MOVABLE_BLOCK = 10;

    private int currentObject = 0;

    [SerializeField] GameObject wall;
    [SerializeField] GameObject playerStart;
    [SerializeField] GameObject playerEnd;
    [SerializeField] GameObject key;
    [SerializeField] GameObject door;
    [SerializeField] GameObject movable;
    [SerializeField] SpriteRenderer sr;


    private void cycleObject()
    {
        for (int i = 0; i < getContainedObjects().Count; i++){
            var obj = getContainedObjects()[i];
            if (obj != null) { removeObject(obj); }
        }
        currentObject += 1;
        currentObject = currentObject % TOTAL_TYPES;
        GameObject newObject;
        switch (currentObject)
        {
            case WALL:
                newObject = Instantiate(wall);
                addObject(newObject); break;
            case PLAYER_START:
                newObject = Instantiate(playerStart);
                addObject(newObject); break;
            case PLAYER_END:
                newObject = Instantiate(playerEnd);
                addObject(newObject); break;
            case KEY_LINK_0:
                newObject = Instantiate(key);
                newObject.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                addObject(newObject); break;
            case DOOR_LINK_0:
                newObject= Instantiate(door);
                newObject.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                addObject(newObject); break;
            case KEY_LINK_1:
                newObject = Instantiate(key);
                newObject.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                addObject(newObject); break;
            case DOOR_LINK_1:
                newObject = Instantiate(door);
                newObject.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                addObject(newObject); break;
            case KEY_LINK_2:
                newObject = Instantiate(key);
                newObject.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                addObject(newObject); break;
            case DOOR_LINK_2:
                newObject = Instantiate(door);
                newObject.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                addObject(newObject); break;
            case MOVABLE_BLOCK:
                newObject = Instantiate(movable);
                addObject(newObject); break;
        }
    }

    private void OnMouseDown()
    {
        cycleObject();
    }

    private void OnMouseOver()
    {
        sr.color = Color.gray;
    }

    private void OnMouseExit() 
    {
        sr.color = Color.white;
    }

    public int getObjectAsInt()
    {
        return currentObject;
    }
}

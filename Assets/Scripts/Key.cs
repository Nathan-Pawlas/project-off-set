using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, Interaction
{
    private int linkNumber;
    private List<Door> doors = new List<Door>();

    public void interact()
    {
        foreach(Door door in doors)
        {
            door.open();
        }
    }

    public void stopInteract()
    {
        foreach (Door door in doors)
        {
            door.close();
        }
    }

    public void addDoor(Door door)
    {
        doors.Add(door);
    }

    public List<Door> getDoor()
    {
        return this.doors;
    }

    public void setLinkNumber(int linkNumber)
    {
        this.linkNumber = linkNumber;
    }

    public int getLinkNumber()
    {
        return this.linkNumber;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private int linkNumber;
    public bool isClosed = true;

    public void open()
    {
        gameObject.SetActive(false);
        isClosed = false;
    }

    public void close()
    {
        gameObject.SetActive(true);
        isClosed= true;
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
